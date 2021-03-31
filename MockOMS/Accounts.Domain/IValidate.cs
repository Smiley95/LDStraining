using System;
using System.Collections.Generic;
using System.Text;

namespace CommandReceiver.Commands
{
    public interface IValidate
    {
        bool TryValidate(out string error);
    }
}
