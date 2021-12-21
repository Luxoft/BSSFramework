//using System;

//using Framework.Core;
//using Framework.OData;
//using Framework.Transfering;

//namespace Framework.DomainDriven
//{
//    public abstract class FetchBuildRule<TDomainObject> : IEquatable<FetchBuildRule<TDomainObject>>
//    {
//        public abstract bool Equals(FetchBuildRule<TDomainObject> other);

//        public override bool Equals(object obj)
//        {
//            return this.Equals(obj as FetchBuildRule<TDomainObject>);
//        }

//        public override int GetHashCode()
//        {
//            return 0;
//        }
//    }




//    public class ODataFetchBuildRule<TDomainObject> : FetchBuildRule<TDomainObject>,
//        IEquatable<ODataFetchBuildRule<TDomainObject>>
//    {
//        public readonly SelectOperation<TDomainObject> SelectOperation;


//        public ODataFetchBuildRule(SelectOperation<TDomainObject> selectOperation)
//        {
//            if (selectOperation == null) throw new ArgumentNullException("selectOperation");

//            this.SelectOperation = selectOperation;
//        }


//        public override bool Equals(FetchBuildRule<TDomainObject> other)
//        {
//            return this.Equals(other as ODataFetchBuildRule<TDomainObject>);
//        }

//        public override int GetHashCode()
//        {
//            return this.SelectOperation.GetHashCode();
//        }

//        public bool Equals(ODataFetchBuildRule<TDomainObject> other)
//        {
//            return other != null && this.SelectOperation.Equals(other.SelectOperation);
//        }
//    }


//    public class MainFetchBuildRule<TDomainObject> : FetchBuildRule<TDomainObject>, IEquatable<MainFetchBuildRule>
//    {
//        public readonly MainDTOType DTOType;

//        public readonly Type ProjectionType;


//        public MainFetchBuildRule(MainDTOType dtoType, Type projectionType = null)
//        {
//            this.DTOType = dtoType;
//            this.ProjectionType = projectionType;
//        }


//        public override bool Equals(FetchBuildRule<TDomainObject> other)
//        {
//            return this.Equals(other as MainFetchBuildRule);
//        }

//        public override int GetHashCode()
//        {
//            return this.DTOType.GetHashCode() ^ this.ProjectionType.TryGetHashCode();
//        }

//        public bool Equals(MainFetchBuildRule<TDomainObject> other)
//        {
//            return other != null && this.DTOType == other.DTOType && this.ProjectionType == other.ProjectionType;
//        }


//        public class ProjectionFetchBuildRule<TDomainObject, TProjection> : MainFetchBuildRule<TDomainObject>,
//            IEquatable<ProjectionFetchBuildRule<TDomainObject, TProjection>>
//            where TDomainObject : TProjection
//        {
//            public readonly MainDTOType DTOType;

//            public readonly Type ProjectionType;


//            public ProjectionFetchBuildRule(MainDTOType dtoType, Type projectionType = null)
//            {
//                this.DTOType = dtoType;
//                this.ProjectionType = projectionType;
//            }


//            public override bool Equals(FetchBuildRule<TDomainObject> other)
//            {
//                return this.Equals(other as MainFetchBuildRule);
//            }

//            public override int GetHashCode()
//            {
//                return this.DTOType.GetHashCode() ^ this.ProjectionType.TryGetHashCode();
//            }

//            public bool Equals(MainFetchBuildRule other)
//            {
//                return other != null && this.DTOType == other.DTOType && this.ProjectionType == other.ProjectionType;
//            }

//            public static implicit operator MainFetchBuildRule(MainDTOType dtoType)
//            {
//                return new MainFetchBuildRule(dtoType);
//            }
//        }
//    }
//}