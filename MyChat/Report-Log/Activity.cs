namespace MindLink.Recruitment.MyChat
{
    /// <summary>
    /// Represents an activity of a user in a conversation.
    /// </summary>
    public sealed class Activity
    {
        /// <summary>
        /// SenderID.
        /// </summary>
        public string sender;

        /// <summary>
        /// The number of messages a user sent.
        /// </summary>
        public int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="Activity"/> class.
        /// </summary>
        public Activity(string senderId, int count)
        {
            this.sender = senderId;
            this.count = count;
        }
    }
}
