namespace Framework.Authorization.BLL
{
    [Framework.Validation.DefaultStringMaxLengthValidator]
    public class ApproveCommand
    {
        public string Comment { get; set; }
    }
}