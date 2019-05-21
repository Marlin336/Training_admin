using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training_admin
{
	public class CustomList
	{
		public int id { get; set; }
		public string fname { get; set;}
		public string sname { get; set;}
		public string pname { get; set; }
		public string birthday { get; set; }
		public string mail { get; set; }
		public string login { get; set; }
		public int deposit { get; set; }
		public bool isEnter { get; set; }
		public CustomList(int id, string sname, string fname, string pname, string birthday, string mail, int dep, string login, bool enter)
		{
			this.id = id;
			this.fname = fname;
			this.sname = sname;
			this.pname = pname;
			this.birthday = birthday;
			this.mail = mail;
			this.login = login;
			deposit = dep;
			isEnter = enter;
		}
	}
	public class TrainerList
	{
		public int id { get; set; }
		public string fname { get; set; }
		public string sname { get; set; }
		public string pname { get; set; }
		public string birthday { get; set; }
		public string mail { get; set; }
		public string login { get; set; }
		public int grpcnt { get; set; }
		public TrainerList(int id, string sname, string fname, string pname, string birthday, string mail, string login, int grpcnt)
		{
			this.id = id;
			this.fname = fname;
			this.sname = sname;
			this.pname = pname;
			this.birthday = birthday;
			this.mail = mail;
			this.login = login;
			this.grpcnt = grpcnt;
		}
	}
	public class GroupList
	{
		public int id { get; set; }
		public string trainer { get; set; }
		public string min_age { get; set; }
		public string max_age { get; set; }
		public int cost { get; set; }
		public int sub { get; set; }
		public GroupList(int id, string trainer, string min, string max, int cost, int sub)
		{
			this.id = id;
			this.trainer = trainer;
			this.min_age = min;
			this.max_age = max;
			this.cost = cost;
			this.sub = sub;
		}
	}
}
