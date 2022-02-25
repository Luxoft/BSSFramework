using Framework.Core;

namespace Framework.Configuration.Domain
{
    public class MessageTemplate
    {
        private string code;
        
        public MessageTemplate()
        {
        }
        
        /// <summary>
        /// Уникальный код шаблона
        /// </summary>
        public virtual string Code
        {
            get { return this.code.TrimNull(); }
            set { this.code = value.TrimNull(); }
        }
    }
}
