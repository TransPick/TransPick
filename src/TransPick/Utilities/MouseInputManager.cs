using System.Drawing;
using System.Windows.Forms;
using TransPick.Unmanaged;

namespace TransPick.Utilities
{
    internal class MouseInputManager : IMessageFilter
    {
        Rectangle _boundRect;
        Rectangle _oldRect = Rectangle.Empty;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x201 || m.Msg == 0x202 || m.Msg == 0x203) return true;
            if (m.Msg == 0x204 || m.Msg == 0x205 || m.Msg == 0x206) return true;
            return false;
        }

        internal void EnableMouse()
        {
            Cursor.Clip = _oldRect;
            Cursor.Show();
            Application.RemoveMessageFilter(this);
        }

        internal void DisableMouse()
        {
            _oldRect = Cursor.Clip;

            // Arbitrary location.
            _boundRect = new Rectangle(Display.GetLeft(), Display.GetTop(), Display.GetWidth(), Display.GetHeight());

            Cursor.Clip = _boundRect;
            Cursor.Hide();
            Application.AddMessageFilter(this);
        }
    }
}
