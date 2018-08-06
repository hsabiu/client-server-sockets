using System;

namespace ChatClientGUI
{
    internal class SetTextCallback
    {
        private Action<string> setText;

        public SetTextCallback(Action<string> setText)
        {
            this.setText = setText;
        }
    }
}