using AwoBot.Modules;
using AwoBot.Modules.Core;
using AwoBot.Utils;
using AwoBotDemo.Modules.MessageDeletionModule.Data;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReInject.Implementation.Attributes;
using ReInject.Interfaces;
using ReInject.PostInjectors.BackgroundWorker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoBotDemo.Modules.MessageDeletionModule
{
  [Module("MessageDeleter", "message-deleter", true, typeof(IMessageDeleter))]
  public class MessageDeleter : IModule, IMessageDeleter
  {

    [Inject]
    private DiscordSocketClient _client;

    [Inject]
    private MessageDeletionContext _context;

    [Inject(name: "message-deleter")]
    private IConfiguration _config;

    [Inject]
    private IDependencyContainer _container;

    private ILogger _logger;

    public MessageDeleter(ILoggerFactory factory)
    {
      _logger = factory.CreateLogger<MessageDeleter>();
    }

    public Task Deinitialize()
    {
      _container.SetEventTargetEnabled(this, false);
      _container.SetPostInjectionsEnabled(this, false);
      return Task.CompletedTask;
    }

    public Task Initialize()
    {
      return Task.CompletedTask;
    }

    public Task Reload()
    {
      return Task.CompletedTask;
    }

    public Task Start()
    {
      _container.SetEventTargetEnabled(this, true);
      _container.SetPostInjectionsEnabled(this, true);
      return Task.CompletedTask;
    }

    [InjectEvent("Discord:MessageBulkDeleted")]
    private async Task handleMessagesBulkDeleted(IReadOnlyCollection<Cacheable<IMessage, ulong>> messages, Cacheable<IMessageChannel, ulong> channel)
    {
      await _context.DeleteMessagesAsync(messages.Select(x => x.Id));
    }

    [InjectEvent("Discord:MessageDeleted")]
    private async Task handleMessageDeleted(Cacheable<IMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel)
    {
      await _context.DeleteMessagesAsync(message.Id);
    }

    [InjectEvent("Discord:MessageReceived")]
    private async Task handleMessageReceived(SocketMessage message)
    {
      await _context.AddMessageAsync(message);
    }

    [BackgroundWorker(Schedule = "0/5 * * * *")]
    private async Task deleteRandomMessage()
    {
      var guild = _client.Guilds.ToArray().Random();
      var filters = await _context.GetFiltersForGuildAsync(guild);
      var deletion = await _context.GetRandomMessageAsync(guild);
      var message = await deletion.GetMessageAsync();
      if (message != null && filters.All(x => x.IsDeletionPermitted(message)))
      {
        await message.DeleteAsync();
        _logger.LogInformation($"Deleted random message in channel {deletion.GuildChannel.Name} of guild {guild.Name} written by {message.Author.Username}#{message.Author.Discriminator}");
      }
    }

    public async Task AddFilterAsync(IGuildUser user)
    {
      await _context.UserFilters.AddAsync(new UserFilterModel(user));
      await _context.SaveChangesAsync();
    }

    public async Task AddFilterAsync(ITextChannel channel)
    {
      await _context.ChannelFilters.AddAsync(new ChannelFilterModel(channel));
      await _context.SaveChangesAsync();
    }

    public async Task AddFilterAsync(IRole role)
    {
      await _context.RoleFilters.AddAsync(new RoleFilterModel(role));
      await _context.SaveChangesAsync();
    }

    public async Task RemoveFilterAsync(IGuildUser user)
    {
      var model = await _context.UserFilters.AsAsyncEnumerable().FirstOrDefaultAsync(x => x.GuildUserId == user.Id && x.GuildId == user.GuildId);
      _context.UserFilters.Remove(model);
      await _context.SaveChangesAsync();
    }

    public async Task RemoveFilterAsync(ITextChannel channel)
    {
      var model = await _context.ChannelFilters.AsAsyncEnumerable().FirstOrDefaultAsync(x => x.GuildChannelId == channel.Id);
      _context.ChannelFilters.Remove(model);
      await _context.SaveChangesAsync();
    }

    public async Task RemoveFilterAsync(IRole role)
    {
      var model = await _context.RoleFilters.AsAsyncEnumerable().FirstOrDefaultAsync(x => x.GuildRoleId == role.Id);
      _context.RoleFilters.Remove(model);
      await _context.SaveChangesAsync();
    }

    public async Task RemoveFilterAsync(IDeletionFilter filter)
    {
      if (filter is ChannelFilterModel channel)
        _context.ChannelFilters.Remove(channel);

      if (filter is RoleFilterModel role)
        _context.RoleFilters.Remove(role);

      if (filter is UserFilterModel user)
        _context.UserFilters.Remove(user);

      await _context.SaveChangesAsync();
    }

    public Task<IEnumerable<IDeletionFilter>> GetFilters(IGuild guild) => _context.GetFiltersForGuildAsync(guild);
  }
}
