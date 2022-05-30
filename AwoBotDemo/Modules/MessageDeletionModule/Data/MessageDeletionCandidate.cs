using AwoBot.BaseDataModels;
using Discord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoBotDemo.Modules.MessageDeletionModule.Data
{
  public class MessageDeletionCandidate : GuildChannelModel
  {
    [NotMapped]
    private IMessage _message;

    [NotMapped]
    public IMessage Message
    {
      get
      {
        return GetMessageAsync().Result;
      }
      set
      {
        MessageId = value?.Id;
        GuildChannel = value?.Channel as IGuildChannel;
        _message = value;
      }
    }
    public ulong? MessageId { get; set; }

    public MessageDeletionCandidate()
    {
    }

    public MessageDeletionCandidate(IMessage message) : base(message.Channel as IGuildChannel)
    { 
      MessageId = message?.Id;
      _message = message;
    }

    public async Task<IMessage> GetMessageAsync()
    {
      if (!MessageId.HasValue)
      {
        return null;
      }

      if (_message == null)
      {
        var channel = await GetGuildChannelAsync();
        if (channel is ITextChannel text)
          _message = await text.GetMessageAsync(MessageId.Value);
      }

      return _message;
    }
  }
}
