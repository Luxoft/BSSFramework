namespace Framework.DomainDriven
{
    public static class MappingObjectExtensions
    {
        /// <summary>
        /// Maps DTO into domain object using mapping service specified
        /// </summary>
        /// <param name="domainObject">Domain object to map to</param>
        /// <param name="mappingObject">DTO instance to map from</param>
        /// <param name="mappingService">Mapping service</param>
        /// <returns>Correctly mapped original domain object</returns>
        public static TDomainObject WithMap<TMappingService, TDomainObject>(
            this TDomainObject domainObject,
            IMappingObject<TMappingService, TDomainObject> mappingObject,
            TMappingService mappingService)
        {
            mappingObject.MapToDomainObject(mappingService, domainObject);

            return domainObject;
        }
    }
}
