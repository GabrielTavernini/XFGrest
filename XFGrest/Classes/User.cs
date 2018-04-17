using System;
namespace XFGrest
{
    public class User
    {
        public User istance { get { return this; } }
        public String name { get; set; }
        public int eta { get; set; }
        public String displayString { get { return String.Format("{0} ({1})", name, eta); }}
        public int lab { get; set; }
        public Boolean[] presences { get; set; } = new Boolean[4];
        public int[] sports { get; set; } = new int[4];
        public Boolean presence { get { return presences[App.Day]; } set { presences[App.Day] = value; } }
        public String sport { get { return App.sports[sports[App.Day]]; } set{ sports[App.Day] = App.sports.FindIndex(a => a == value); } }
        public int ID { get; set; }

        public User(Boolean save)
        {
            if(save)
            {
                ID = App.Users.Count;
                App.Users.Add(this); 
            }
        }

        public void Save()
        {
            ID = App.Users.Count;
            App.Users.Add(this); 
        }


        public void Remove()
        {
            App.Users.Remove(this);
        }
    }
}
