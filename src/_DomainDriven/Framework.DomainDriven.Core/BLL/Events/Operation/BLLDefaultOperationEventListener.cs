using System;
using System.Collections.Generic;

namespace Framework.DomainDriven.BLL
{
    internal sealed class BLLDefaultOperationEventListener<TDomainObject> : BLLOperationEventListener<TDomainObject, BLLBaseOperation>

        where TDomainObject : class
    {
        public BLLDefaultOperationEventListener(IDictionary<Type, IDictionary<Type, BLLOperationEventListener>> cache)
            : base(cache)
        {
        }


        internal override IEnumerable<KeyValuePair<Type, KeyValuePair<Type, BLLOperationEventListener>>> GetOtherEventListeners()
        {
            return this.GetDefaultOtherEventListeners();
        }

        //internal override IEnumerable<KeyValuePair<Type, BLLOperationEventListener>> GetOtherEventListeners()
        //{
        //    return this.GetDefaultOtherEventListeners();
        //    //var request1 = from pairListener in this.Cache

        //    //               where pairListener.Key.IsAssignableFrom(typeof(TDomainObject))

        //    //               from listener in pairListener.Value

        //    //               where listener.

        //    //               sele




        //    //return this.Cache[typeof (TDomainObject)]
        //    //           .Where(listenerPair => listenerPair.Value != this)
        //    //           .SelectMany(otherListener => otherListener.Value.GetDefaultOtherEventListeners())
        //    //           .Distinct(pair => pair.Value);
        //}
    }
}

   //               TDomain | BLLBaseOperation

   //                /                      \
   //               /                        \
   //              /                          \
   //             /                            \
   //            /                              \
   //           /                                \
   //          /                                  \
   //         /                                    \

   //Invoice | BLLBaseOperation             TDomain | TOperation

   //         \                                    /
   //          \                                  /
   //           \                                /
   //            \                              /
   //             \                            /
   //              \                          /
   //               \                        /
   //                \                      /

   //                 Invoice  |  TOperation