using System;

namespace Infrastructure.Data.Seedwork.Declaration
{
    [Serializable]
    public class Response
    {
        public enum ResponseActionKind : byte
        {
            ServerShutdown,
            ForceLogoff,
            Refresh,
            NoticeShow
        }

        public ApplicationMessageKind Code { get; private set; }
        public string Message { get; set; }

        public ResponseActionKind? Action { get; private set; }

        public Type TargetType { get; private set; }

        //public T Data { get; private set; }
        public object Data { get; set; }

        public Response()
        {
            Code = ApplicationMessageKind.Info;
            Action = null;
            Message = null;
        }

        public Response(ApplicationMessageKind code, string msg)
        {
            Code = code;
            Message = msg;
            Action = null;
        }

        public Response(ApplicationMessageKind code, ResponseActionKind action, string msg)
        {
            Code = code;
            Message = msg;

            Action = action;
        }

        public Response(Type targetType)
        {
            Code = ApplicationMessageKind.Critial;
            Action = Response.ResponseActionKind.Refresh;
            TargetType = targetType;
            Message = "Reload";
        }
    }

    public class QueueItem
    {
        public bool Done { get; set; }
        public Guid? Exclude { get; set; }

        public Response Data { get; set; }

        public bool POSOnly { get; set; }
    }
}
