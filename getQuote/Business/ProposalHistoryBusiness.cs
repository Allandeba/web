﻿using getQuote.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace getQuote
{
    public class ProposalHistoryBusiness
    {
        private readonly ProposalHistoryRepository _repository;

        private readonly ItemBusiness _itemBusiness;

        public ProposalHistoryBusiness(
            ProposalHistoryRepository ProposalHistoryRepository,
            ItemBusiness itemBusiness
        )
        {
            _repository = ProposalHistoryRepository;
            _itemBusiness = itemBusiness;
        }

        public async Task<ProposalModel> GetProposalFromHistory(int proposalHistoryId)
        {
            ProposalHistoryModel proposalHistory = await GetByIdAsync(proposalHistoryId);

            ProposalModel proposal = new ProposalModel
            {
                Person = proposalHistory.Person,
                ProposalContent = new List<ProposalContentModel>(),
            };

            IEnumerable<ItemModel> items = await GetAllItemsAsync(proposalHistory);
            proposalHistory.ProposalContentJSON.ProposalContentItems.OrderBy(p => p.ItemId);
            items.OrderBy(i => i.ItemId);

            for (int i = 0; i < items.Count(); i++)
            {
                ProposalContentModel proposalContent = new ProposalContentModel
                {
                    Proposal = proposal,
                    Item = items.ElementAt(i),
                    Quantity = Int32.Parse(
                        proposalHistory.ProposalContentJSON.ProposalContentItems[i].Quantity
                    ),
                };

                proposal.ProposalContent.Add(proposalContent);
            }

            return proposal;
        }

        private async Task<IEnumerable<ItemModel>> GetAllItemsAsync(
            ProposalHistoryModel proposalHistory
        )
        {
            ItemIncludes[] includes = new ItemIncludes[] { ItemIncludes.ItemImage };

            IEnumerable<ItemModel> items = await _itemBusiness.FindAsync(
                i => proposalHistory.ProposalContentJSON.GetItemIds().Contains(i.ItemId),
                includes
            );
            return items;
        }

        public async Task AddAsync(ProposalHistoryModel proposalHistory)
        {
            await _repository.AddAsync(proposalHistory);
        }

        public async Task<ProposalHistoryModel> GetByIdAsync(int proposalHistoryId)
        {
            ProposalHistoryIncludes[] includes = new ProposalHistoryIncludes[]
            {
                ProposalHistoryIncludes.Person,
                ProposalHistoryIncludes.Proposal
            };

            return await _repository.GetByIdAsync(
                proposalHistoryId,
                includes.Cast<System.Enum>().ToArray()
            );
        }
    }
}
