using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using Cointributors.Web.Client.Contract.CointributionSplitter.ContractDefinition;

namespace Cointributors.Web.Client.Contract.CointributionSplitter
{
    public partial class CointributionSplitterService: ContractWeb3ServiceBase
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.IWeb3 web3, CointributionSplitterDeployment cointributionSplitterDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<CointributionSplitterDeployment>().SendRequestAndWaitForReceiptAsync(cointributionSplitterDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.IWeb3 web3, CointributionSplitterDeployment cointributionSplitterDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<CointributionSplitterDeployment>().SendRequestAsync(cointributionSplitterDeployment);
        }

        public static async Task<CointributionSplitterService> DeployContractAndGetServiceAsync(Nethereum.Web3.IWeb3 web3, CointributionSplitterDeployment cointributionSplitterDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, cointributionSplitterDeployment, cancellationTokenSource);
            return new CointributionSplitterService(web3, receipt.ContractAddress);
        }

        public CointributionSplitterService(Nethereum.Web3.IWeb3 web3, string contractAddress) : base(web3, contractAddress)
        {
        }

        public Task<string> DonateRequestAsync(Donate1Function donate1Function)
        {
             return ContractHandler.SendRequestAsync(donate1Function);
        }

        public Task<TransactionReceipt> DonateRequestAndWaitForReceiptAsync(Donate1Function donate1Function, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(donate1Function, cancellationToken);
        }

        public Task<string> DonateRequestAsync(string sender, BigInteger amount, string tokenAddress)
        {
            var donate1Function = new Donate1Function();
                donate1Function.Sender = sender;
                donate1Function.Amount = amount;
                donate1Function.TokenAddress = tokenAddress;
            
             return ContractHandler.SendRequestAsync(donate1Function);
        }

        public Task<TransactionReceipt> DonateRequestAndWaitForReceiptAsync(string sender, BigInteger amount, string tokenAddress, CancellationTokenSource cancellationToken = null)
        {
            var donate1Function = new Donate1Function();
                donate1Function.Sender = sender;
                donate1Function.Amount = amount;
                donate1Function.TokenAddress = tokenAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(donate1Function, cancellationToken);
        }

        public Task<string> DonateRequestAsync(DonateFunction donateFunction)
        {
             return ContractHandler.SendRequestAsync(donateFunction);
        }

        public Task<TransactionReceipt> DonateRequestAndWaitForReceiptAsync(DonateFunction donateFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(donateFunction, cancellationToken);
        }

        public Task<string> DonateRequestAsync(BigInteger amount, string tokenAddress)
        {
            var donateFunction = new DonateFunction();
                donateFunction.Amount = amount;
                donateFunction.TokenAddress = tokenAddress;
            
             return ContractHandler.SendRequestAsync(donateFunction);
        }

        public Task<TransactionReceipt> DonateRequestAndWaitForReceiptAsync(BigInteger amount, string tokenAddress, CancellationTokenSource cancellationToken = null)
        {
            var donateFunction = new DonateFunction();
                donateFunction.Amount = amount;
                donateFunction.TokenAddress = tokenAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(donateFunction, cancellationToken);
        }

        public Task<GetRecipientOutputDTO> GetRecipientQueryAsync(GetRecipientFunction getRecipientFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetRecipientFunction, GetRecipientOutputDTO>(getRecipientFunction, blockParameter);
        }

        public Task<GetRecipientOutputDTO> GetRecipientQueryAsync(BigInteger index, BlockParameter blockParameter = null)
        {
            var getRecipientFunction = new GetRecipientFunction();
                getRecipientFunction.Index = index;
            
            return ContractHandler.QueryDeserializingToObjectAsync<GetRecipientFunction, GetRecipientOutputDTO>(getRecipientFunction, blockParameter);
        }

        public Task<RecipientsOutputDTO> RecipientsQueryAsync(RecipientsFunction recipientsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<RecipientsFunction, RecipientsOutputDTO>(recipientsFunction, blockParameter);
        }

        public Task<RecipientsOutputDTO> RecipientsQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var recipientsFunction = new RecipientsFunction();
                recipientsFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryDeserializingToObjectAsync<RecipientsFunction, RecipientsOutputDTO>(recipientsFunction, blockParameter);
        }

        public override List<Type> GetAllFunctionTypes()
        {
            return new List<Type>
            {
                typeof(Donate1Function),
                typeof(DonateFunction),
                typeof(GetRecipientFunction),
                typeof(RecipientsFunction)
            };
        }

        public override List<Type> GetAllEventTypes()
        {
            return new List<Type>
            {
                typeof(DonationReceivedEventDTO)
            };
        }

        public override List<Type> GetAllErrorTypes()
        {
            return new List<Type>
            {

            };
        }
    }
}
