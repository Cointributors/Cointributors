using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Cointributors.Web.Client.Contract.CointributionSplitter.ContractDefinition
{
    public partial class Recipient : RecipientBase { }

    public class RecipientBase 
    {
        [Parameter("address", "recipientAddress", 1)]
        public virtual string RecipientAddress { get; set; }
        [Parameter("uint256", "portion", 2)]
        public virtual BigInteger Portion { get; set; }
        [Parameter("bool", "isSplitter", 3)]
        public virtual bool IsSplitter { get; set; }
    }
}
