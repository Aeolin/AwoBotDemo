using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoBotDemo.Modules.MessageDeletionModule.Data
{
  public interface IDeletionFilter
  {
    public bool IsDeletionPermitted(IMessage message);
    public bool Enabled { get; set; }
    public string DisplayText { get; }
  }
}
