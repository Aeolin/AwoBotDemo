using AwoBot.BaseDataModels;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReInject.Implementation.Attributes;
using ReInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoBotDemo.Modules.MessageDeletionModule.Data
{
  public class MessageDeletionContext : AwoBotContext
  {

    [Inject]
    private IConfiguration _config;

    public MessageDeletionContext(IDependencyContainer container) : base(container)
    {
    }

    public DbSet<ChannelFilterModel> ChannelFilters { get; set; }
    public DbSet<RoleFilterModel> RoleFilters { get; set; }
    public DbSet<UserFilterModel> UserFilters { get; set; }

    public DbSet<MessageDeletionCandidate> Messages { get; set; }

    public Task<MessageDeletionCandidate> GetRandomMessageAsync(IGuild guild) => GetRandomMessageAsync(guild.Id);
    public async Task<MessageDeletionCandidate> GetRandomMessageAsync(ulong guildId)
    {
      return await Messages.AsAsyncEnumerable()
        .Where(x => x.GuildId == guildId)
        .OrderBy(x => Guid.NewGuid())
        .FirstAsync();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
      _container?.PostInject(this);
      optionsBuilder.UseLazyLoadingProxies();
      if (optionsBuilder.IsConfigured == false)
        optionsBuilder.UseSqlite(_config["access:database"]);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      foreach (var entity in modelBuilder.Model.GetEntityTypes())
        entity.SetTableName($"MessageDeletion.{entity.GetTableName()}");
    }

    public Task<IEnumerable<IDeletionFilter>> GetFiltersForGuildAsync(IGuild guild) => GetFiltersForGuildAsync(guild.Id);


    public async Task<IEnumerable<IDeletionFilter>> GetFiltersForGuildAsync(ulong guildId)
    {
      return await ChannelFilters.AsAsyncEnumerable().Where(x => x.GuildId == guildId)
        .Concat<IDeletionFilter>(RoleFilters.AsAsyncEnumerable().Where(x => x.GuildId == guildId))
        .Concat<IDeletionFilter>(UserFilters.AsAsyncEnumerable().Where(x => x.GuildId == guildId))
        .ToArrayAsync();
    }

    public async Task AddMessageAsync(IMessage message)
    {
      if (message.Channel is ITextChannel)
      {
        var model = new MessageDeletionCandidate(message);
        await Messages.AddAsync(model);
        await SaveChangesAsync();
      }
    }

    public Task DeleteMessagesAsync(IEnumerable<ulong> messages)
    {
      return DeleteMessagesAsync(messages.ToArray());
    }

    public async Task DeleteMessagesAsync(params ulong[] messages)
    {
      var toDelete = await Messages.AsAsyncEnumerable().Where(x => messages.Contains(x.MessageId ?? 0)).ToArrayAsync();
      Messages.RemoveRange(toDelete);
      await SaveChangesAsync();
    }
  }
}
