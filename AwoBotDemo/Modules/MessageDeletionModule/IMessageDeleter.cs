using AwoBotDemo.Modules.MessageDeletionModule.Data;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoBotDemo.Modules.MessageDeletionModule
{
  public interface IMessageDeleter
  {
    public Task AddFilterAsync(IGuildUser user);
    public Task AddFilterAsync(ITextChannel channel);
    public Task AddFilterAsync(IRole role);
    public Task RemoveFilterAsync(IGuildUser user);
    public Task RemoveFilterAsync(ITextChannel channel);
    public Task RemoveFilterAsync(IRole role);
    public Task RemoveFilterAsync(IDeletionFilter filter);
    public Task<IEnumerable<IDeletionFilter>> GetFilters(IGuild guild);
  }
}
