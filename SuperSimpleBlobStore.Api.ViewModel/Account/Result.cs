using System.Collections.Generic;

namespace SuperSimpleBlobStore.Api.ViewModel
{
    public class Result
    {
        private bool _disabled;

        public Result()
            : this(false, false, false)
        {
        }

        public Result(bool success)
            : this(success, false, false)
        {
        }

        public Result(bool success, bool error)
            : this(success, error, false)
        {

        }

        public Result(bool success, bool error, bool disabled)
        {
            Success = success;
            Error = error;
            Disabled = disabled;

            Notifications = new List<string>();
        }


        public bool Success { get; set; }
        public bool Error { get; set; }

        public bool Disabled
        {
            get { return _disabled; }
            set
            {
                if(value)
                    AddInfo("This account is disabled");

                _disabled = value;
            }
        }

        public List<string> Notifications { get; set; }

        public void AddError(string message)
        {
            Notifications.Add("<div class=\"alert alert-danger\">" + message + "</div>");
        }

        public void AddWarning(string message)
        {
            Notifications.Add("<div class=\"alert alert-warning\">" + message + "</div>");
        }

        public void AddInfo(string message)
        {
            Notifications.Add("<div class=\"alert alert-info\">" + message + "</div>");
        }

        public void AddSuccess(string message)
        {
            Notifications.Add("<div class=\"alert alert-success\">" + message + "</div>");
        }
    }
}
