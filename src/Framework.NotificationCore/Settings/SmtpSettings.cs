namespace Framework.NotificationCore.Settings
{
    public class SmtpSettings
    {
        /// <summary>
        /// false чтобы не отправлять нотификации на смтп сервер (для разработческих нужд в основном)
        /// </summary>
        public bool SmtpEnabled { get; set; }

        public string OutputFolder { get; set; }

        public string Server { get; set; }

        public int Port { get; set; }

        /// <summary>
        /// Ящик, на который будут перенаправлены нотификации без получателей (те нотификации, которые без получателей впринципе разрешены)
        /// </summary>
        public string[] DefaultReceiverEmails { get; set; }

        /// <summary>
        /// Тестовый ящик, на который будут перенаправлены нотификации тестовых стендов (которые !environment.IsProduction())
        /// </summary>
        public string[] TestEmails { get; set; }
    }
}
