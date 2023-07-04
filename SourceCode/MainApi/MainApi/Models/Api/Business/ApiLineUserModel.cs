using System.Collections.Generic;

namespace MainApi.Models.Api.Business
{
    #region Error
    public class LineMessageErrorDetailModel
    {
        public string message { get; set; }
        public string property { get; set; }
    }

    public class LineMessageErrorModel
    {
        public string message { get; set; }
        public List<LineMessageErrorDetailModel> details { get; set; }
    }
    #endregion

    #region TextMessage
    public class MessageTemplateDefaultActionModel
    {
        public string type { get; set; }
        public string label { get; set; }
        public string uri { get; set; }
    }

    public class MessageTemplateActionModel
    {
        public string type { get; set; }
        public string label { get; set; }
        public string data { get; set; }
        public string uri { get; set; }
    }

    public class MessageTemplateModel
    {
        public string type { get; set; }
        public string thumbnailImageUrl { get; set; }
        public string imageAspectRatio { get; set; }
        public string imageSize { get; set; }
        public string imageBackgroundColor { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public MessageTemplateDefaultActionModel defaultAction { get; set; }
        public List<MessageTemplateActionModel> actions { get; set; }
    }

    public class MessageInfoModel
    {
        public string type { get; set; }
        public string altText { get; set; }
        public MessageTemplateModel template { get; set; }
        public string text { get; set; }
    }

    public class PushMessageModel
    {
        public string to { get; set; }
        public List<MessageInfoModel> messages { get; set; }
    }

    public class PushMessageMulticastModel
    {
        public List<string> to { get; set; }
        public List<MessageInfoModel> messages { get; set; }
    }

    public class ReplyMessageModel
    {
        public string replyToken { get; set; }
        public List<MessageInfoModel> messages { get; set; }
    }
    #endregion
}
