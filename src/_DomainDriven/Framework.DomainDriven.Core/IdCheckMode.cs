namespace Framework.DomainDriven.BLL
{
    public enum IdCheckMode
    {
        /// <summary>
        /// Don't throw exception if object not found
        /// </summary>
        DontCheck = 0,

        /// <summary>
        /// Throw exception if object not found
        /// </summary>
        CheckAll = 1,

        /// <summary>
        /// Return null in Id is empty or throw exception if object not found
        /// </summary>
        SkipEmpty = 2
    }
}