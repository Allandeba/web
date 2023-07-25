﻿using getQuote.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace getQuote
{
    public class ProposalBusiness
    {
        private readonly ProposalRepository _repository;
        private readonly PersonBusiness _personBusiness;
        private readonly ItemBusiness _itemBusiness;

        public ProposalBusiness(
            ProposalRepository ProposalRepository,
            PersonBusiness personBusiness,
            ItemBusiness itemBusiness
        )
        {
            _repository = ProposalRepository;
            _personBusiness = personBusiness;
            _itemBusiness = itemBusiness;
        }

        public async Task<IEnumerable<ProposalModel>> GetProposals()
        {
            ProposalIncludes[] includes = new ProposalIncludes[]
            {
                ProposalIncludes.Person,
                ProposalIncludes.ProposalHistory
            };
            IEnumerable<ProposalModel> proposals = await _repository.GetAllAsync(
                includes.Cast<System.Enum>().ToArray()
            );
            return proposals.OrderByDescending(p => p.ProposalId);
        }

        public async Task AddAsync(ProposalModel proposal)
        {
            proposal.Person = await GetPersonByIdAsync(proposal.PersonId);

            foreach (var proposalContent in proposal.ProposalContent)
            {
                proposalContent.Item = await _itemBusiness.GetByIdAsync(proposalContent.ItemId);
            }
            ;

            await _repository.AddAsync(proposal);
        }

        public async Task<ProposalModel> GetByIdAsync(int proposalId)
        {
            ProposalIncludes[] includes = new ProposalIncludes[]
            {
                ProposalIncludes.Person,
                ProposalIncludes.Item
            };
            return await _repository.GetByIdAsync(
                proposalId,
                includes.Cast<System.Enum>().ToArray()
            );
        }

        public async Task<ProposalModel> GetPrintByIdAsync(int proposalId)
        {
            ProposalIncludes[] includes = new ProposalIncludes[]
            {
                ProposalIncludes.Person,
                ProposalIncludes.ItemImageList
            };
            return await _repository.GetByIdAsync(
                proposalId,
                includes.Cast<System.Enum>().ToArray()
            );
        }

        public async Task UpdateAsync(ProposalModel proposal)
        {
            proposal.Person = await _personBusiness.GetByIdAsync(proposal.PersonId);

            foreach (ProposalContentModel proposalContent in proposal.ProposalContent)
            {
                proposalContent.Item = await _itemBusiness.GetByIdAsync(proposalContent.ItemId);
            }

            ProposalIncludes[] includes = new ProposalIncludes[]
            {
                ProposalIncludes.Person,
                ProposalIncludes.ItemImageList
            };
            ProposalModel? existentProposal = await _repository.GetByIdAsync(
                proposal.ProposalId,
                includes.Cast<System.Enum>().ToArray()
            );

            if (existentProposal == null)
            {
                return;
            }

            //SetProposalHistoryAsync(existentProposal);

            await UpdateExistentProposalInformation(existentProposal, proposal);
            await _repository.UpdateAsync(existentProposal);
        }

        public async Task RemoveAsync(int proposalId)
        {
            ProposalIncludes[] includes = new ProposalIncludes[]
            {
                ProposalIncludes.Person,
                ProposalIncludes.ItemImageList
            };
            ProposalModel? proposal = await _repository.GetByIdAsync(
                proposalId,
                includes.Cast<System.Enum>().ToArray()
            );
            if (proposal == null)
            {
                return;
            }
            ;

            foreach (ProposalContentModel proposalContent in proposal.ProposalContent)
            {
                ProposalContentModel dBProposalContent =
                    await _repository.GetProposalContentByIdAsync(
                        proposalContent.ProposalContentId
                    );
                await _repository.RemoveProposalContentAsync(dBProposalContent);
            }

            await _repository.RemoveAsync(proposal);
        }

        private async Task<PersonModel> GetPersonByIdAsync(int personId)
        {
            return await _personBusiness.GetByIdAsync(personId);
        }

        private async Task SetProposalHistoryAsync(ProposalModel existentProposal)
        {
            // Todo: Quando fizer o ProposalHistoryBusiness passar pra lá
            //ProposalHistoryModel ProposalHistory = new();
            //ProposalHistory.ModificationDate = existentProposal.ModificationDate;
            //ProposalHistory.Person = existentProposal.Person;
            //ProposalHistory.Proposal = existentProposal;

            //ProposalContentJSON proposalContentJSON = new();
            //foreach (ProposalContentModel proposalContent in existentProposal.ProposalContent) {
            //    ProposalContentItems proposalContentItems = new();
            //    proposalContentItems.ItemId = proposalContent.ItemId.ToString();
            //    proposalContentItems.Quantity = proposalContent.Quantity.ToString();
            //    proposalContentJSON.ProposalContentItems.Add(proposalContentItems);
            //}

            //ProposalHistory.ProposalContentJSON = proposalContentJSON;

            //await _proposalHistoryBusiness.AddAsync(ProposalHistory);
        }

        private async Task UpdateExistentProposalInformation(
            ProposalModel existentProposal,
            ProposalModel proposalToUpdate
        )
        {
            existentProposal.Person = proposalToUpdate.Person;
            //existentProposal.ProposalContent = proposalToUpdate.ProposalContent;
            //existentProposal.ProposalHistory = proposalToUpdate.ProposalHistory;
            existentProposal.Discount = proposalToUpdate.Discount;

            await RemoveDeletedProposalContents(existentProposal, proposalToUpdate);

            // Adiciona novos itens ou atualiza os existentes com seus novos valores
            //foreach (var proposalContent in proposal.ProposalContent) {
            //    var updateProposalContent = existentProposal.ProposalContent.Find(
            //        a => a.Item.ItemId == proposalContent.Item.ItemId
            //    );
            //    if (updateProposalContent == null) {
            //        existentProposal.ProposalContent.Add(proposalContent);
            //    } else {
            //        updateProposalContent.Item = proposalContent.Item;
            //        updateProposalContent.Quantity = proposalContent.Quantity;
            //    }
            //}
        }

        private async Task RemoveDeletedProposalContents(
            ProposalModel existentProposal,
            ProposalModel proposalToUpdate
        )
        {
            // ProposalContent enviados da view
            List<int> updatedProposalContentIds = proposalToUpdate.ProposalContent
                .Select(pc => pc.ProposalContentId)
                .ToList();

            // Pega todos os ProposalContent que estão no banco mas foram excluidos na view
            IEnumerable<ProposalContentModel> proposalContentToExclude =
                await _repository.FindProposalContentAsync(
                    pc =>
                        !updatedProposalContentIds.Contains(pc.ProposalContentId)
                        && pc.ProposalId == proposalToUpdate.ProposalId
                );
            foreach (ProposalContentModel existentProposalContent in proposalContentToExclude)
            {
                existentProposal.ProposalContent.Remove(existentProposalContent);
            }
        }

        public async Task<SelectList> GetSelectListPeople()
        {
            IEnumerable<PersonModel> people = await _personBusiness.GetPeople();
            return new SelectList(people, "PersonId", "PersonName");
        }

        public async Task<SelectList> GetSelectListItems()
        {
            IEnumerable<ItemModel> items = await _itemBusiness.GetItems();
            return new SelectList(items, "ItemId", "ItemName");
        }

        public async Task<IEnumerable<dynamic>> GetDynamicItems()
        {
            IEnumerable<ItemModel> items = await _itemBusiness.GetItems();
            return items
                .Select(
                    i =>
                        new
                        {
                            ItemId = i.ItemId,
                            ItemName = i.ItemName,
                            Value = i.Value,
                        }
                )
                .ToList<dynamic>();
        }
    }
}
