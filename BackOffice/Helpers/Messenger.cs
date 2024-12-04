using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Helpers
{
    public class Messenger : ValueChangedMessage<string>
    {
        public Messenger(string value) : base(value) { }
    }
}
