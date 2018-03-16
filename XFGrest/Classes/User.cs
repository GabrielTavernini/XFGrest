using System;
namespace XFGrest
{
    public class User
    {
        public String name { get; set; }
        public int eta { get; set; }
        public String displayString { get { return String.Format("{0} ({1})", name, eta); }}
        public String lab { get; set; }
        public Boolean[] presences { get; set; } = new Boolean[4];
        public String[] sports { get; set; } = new String[4];
        public Boolean presence { get { return presences[App.Day]; } set{} }
        public String sport { get { return sports[App.Day]; } set{} }

        public User(Boolean save)
        {
            if(save)
                App.Users.Add(this); 
        }
    }
}
