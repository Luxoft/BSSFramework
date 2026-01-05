namespace Framework.DomainDriven.BLL;

public interface IBLLBase<out TBLLContext, TDomainObject> : IBLLQueryBase<TDomainObject>, IBLLContextContainer<TBLLContext>;
