using System;

namespace Framework.DomainDriven.DAL.Revisions
{
    public abstract class RevisionInfoBase
    {
        private readonly AuditRevisionType _revisionType;
        private readonly string _author;
        private readonly DateTime _date;
        private readonly long _revisionNumber;

        protected RevisionInfoBase(AuditRevisionType revisionType, string author, DateTime date, long revisionNumber)
        {
            this._revisionType = revisionType;
            this._author = author;
            this._date = date;
            this._revisionNumber = revisionNumber;
        }

        public AuditRevisionType RevisionType
        {
            get { return this._revisionType; }
        }
        public string Author
        {
            get { return this._author; }
        }
        public DateTime Date
        {
            get { return this._date; }
        }

        public long RevisionNumber
        {
            get { return this._revisionNumber; }
        }
    }
}
