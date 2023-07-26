namespace getQuote.Models
{
    public enum DocumentTypes : int
    {
        CPF,
        RG
    }

    public enum SelectDefault
    {
        None
    }

    public enum ProposalIncludes
    {
        None,
        Person,
        ProposalHistory,
        Item,
        ItemImageList
    }

    public enum PersonIncludes
    {
        None,
        Contact,
        Document
    }

    public enum ItemIncludes
    {
        None,
        ItemImage
    }

    public enum ProposalHistoryIncludes
    {
        None,
        Person,
        Proposal
    }
}
