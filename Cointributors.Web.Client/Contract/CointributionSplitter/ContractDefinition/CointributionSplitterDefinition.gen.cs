using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace Cointributors.Web.Client.Contract.CointributionSplitter.ContractDefinition
{


    public partial class CointributionSplitterDeployment : CointributionSplitterDeploymentBase
    {
        public CointributionSplitterDeployment() : base(BYTECODE) { }
        public CointributionSplitterDeployment(string byteCode) : base(byteCode) { }
    }

    public class CointributionSplitterDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public CointributionSplitterDeploymentBase() : base(BYTECODE) { }
        public CointributionSplitterDeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("tuple[]", "_recipients", 1)]
        public virtual List<Recipient> Recipients { get; set; }
    }

    public partial class DonateFunction : DonateFunctionBase { }

    [Function("donate")]
    public class DonateFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amount", 1)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("address", "tokenAddress", 2)]
        public virtual string TokenAddress { get; set; }
    }

    public partial class GetRecipientFunction : GetRecipientFunctionBase { }

    [Function("getRecipient", typeof(GetRecipientOutputDTO))]
    public class GetRecipientFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "index", 1)]
        public virtual BigInteger Index { get; set; }
    }

    public partial class RecipientsFunction : RecipientsFunctionBase { }

    [Function("recipients", typeof(RecipientsOutputDTO))]
    public class RecipientsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class DonationReceivedEventDTO : DonationReceivedEventDTOBase { }

    [Event("DonationReceived")]
    public class DonationReceivedEventDTOBase : IEventDTO
    {
        [Parameter("address", "donor", 1, true )]
        public virtual string Donor { get; set; }
        [Parameter("address", "token", 2, true )]
        public virtual string Token { get; set; }
        [Parameter("uint256", "amount", 3, false )]
        public virtual BigInteger Amount { get; set; }
    }



    public partial class GetRecipientOutputDTO : GetRecipientOutputDTOBase { }

    [FunctionOutput]
    public class GetRecipientOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
        [Parameter("uint256", "", 2)]
        public virtual BigInteger ReturnValue2 { get; set; }
        [Parameter("bool", "", 3)]
        public virtual bool ReturnValue3 { get; set; }
    }

    public partial class RecipientsOutputDTO : RecipientsOutputDTOBase { }

    [FunctionOutput]
    public class RecipientsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "recipientAddress", 1)]
        public virtual string RecipientAddress { get; set; }
        [Parameter("uint256", "portion", 2)]
        public virtual BigInteger Portion { get; set; }
        [Parameter("bool", "isSplitter", 3)]
        public virtual bool IsSplitter { get; set; }
    }
}
