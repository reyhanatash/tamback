using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tamland_API.Models
{
    public class UserHubModel
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
        public string UserGroup { get; set; }
        //if freeflag==0 ==> Busy
        //if freeflag==1 ==> Free
        public string freeflag { get; set; }
        //if tpflag==2 ==> User Admin
        //if tpflag==0 ==> User Member
        //if tpflag==1 ==> Admin
        public string tpflag { get; set; }
        public int UserID { get; set; }
        public int AdminID { get; set; }
        public string Ip { get; set; }
        public string FullName { get; set; }
    }
    public class MessageModel
    {
        public string UserName { get; set; }

        public string Message { get; set; }

        public string UserGroup { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string MsgDate { get; set; }

    }
    public class SendMessage
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string ip { get; set; }
        public string MessageId { get; set; }
        public bool Approved { get; set; }
        public string ConnectionId { get; set; }
        public string FullName { get; set; }
        public string Volume { get; set; }
        public string VolumeStatus { get; set; }
        public bool Deleted { get; set; }
        public int type { get; set; }
        public string Options { get; set; }
        public string QuestionId { get; set; }
        public bool IsAdmin { get; set; }
        public string Replay { get; set; }
        public bool IsEnd { get; set; }
    }

    public class Rootobject
    {
        public string text { get; set; }
        public Option[] options { get; set; }
        public string id { get; set; }
    }

    public class Option
    {
        public string text { get; set; }
        public bool isAnswer { get; set; }
        public string persent { get; set; }
    }


}