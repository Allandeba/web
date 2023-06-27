const NEW_PROPOSAL_CONTENT_ID = 0;

function getItemList() {
    return document.getElementById("ItemList");
}

function setItemDefault() {
    let itemList = getItemList();
    itemList.value = 0;
}

function getProposalContentTable() {
    return document.getElementById("proposalContentTable");
}

function addProposalContentCellClasses(cell) {
    cell.classList.add("text-center");
}

function setProposalContentIdContent(proposalContentIdCell) {
    let itemList = getItemList();
    let input = '<input type="hidden" name="ItemIdList" value="' + itemList.value + '" />';
    let span = '<span>' + NEW_PROPOSAL_CONTENT_ID + '</span>';

    proposalContentIdCell.innerHTML = input + span;
}

function setProposalContentItemIdContent(itemIdCell) {
    let itemList = getItemList();
    itemIdCell.textContent = itemList.value;
}

function setProposalContentItemNameContent(itemNameCell) {
    let itemList = getItemList();
    itemNameCell.textContent = itemList.options[itemList.selectedIndex].text;
}

function setProposalContentActionContent(actionsCell) {
    let itemList = getItemList();
    actionsCell.innerHTML = '<span class="delete-icon" onclick="deleteItem(' + NEW_PROPOSAL_CONTENT_ID + ', ' + itemList.value + ')">' +
        '<svg xmlns="http://www.w3.org/2000/svg" height="1em" viewBox="0 0 448 512">' +
        '<style>svg {fill: #e60a0a}</style>' +
        '<path d="M432 32H312l-9.4-18.7A24 24 0 0 0 281.1 0H166.8a23.72 23.72 0 0 0-21.4 13.3L136 32H16A16 16 0 0 0 0 48v32a16 16 0 0 0 16 16h416a16 16 0 0 0 16-16V48a16 16 0 0 0-16-16zM53.2 467a48 48 0 0 0 47.9 45h245.8a48 48 0 0 0 47.9-45L416 128H32z"/>' +
        '</svg>' +
        '</span>';

    //let span = document.createElement('span');
    //span.classList.add('delete-icon');
    //span.addEventListener('onclick', () => {
    //    deleteItem(NEW_PROPOSAL_CONTENT_ID, itemList.value);
    //});

    //let svg = document.createElement('svg');
    //svg.setAttribute('xmlns', 'http://www.w3.org/2000/svg');
    //svg.setAttribute('height', '1em');
    //svg.setAttribute('viewBox', '0 0 448 512');

    //let style = document.createElement('style');
    //style.innerHTML = 'svg {fill: #e60a0a}';
    //svg.appendChild(style);

    //let path = document.createElement('path');
    //path.setAttribute('d', 'M432 32H312l-9.4-18.7A24 24 0 0 0 281.1 0H166.8a23.72 23.72 0 0 0-21.4 13.3L136 32H16A16 16 0 0 0 0 48v32a16 16 0 0 0 16 16h416a16 16 0 0 0 16-16V48a16 16 0 0 0-16-16zM53.2 467a48 48 0 0 0 47.9 45h245.8a48 48 0 0 0 47.9-45L416 128H32z');
    //svg.appendChild(path);

    //span.appendChild(svg);
    //actionsCell.appendChild(span);
}

function createProposalContentCells(proposalContentRow) {

    let proposalContentIdCell = proposalContentRow.insertCell();
    let itemIdCell = proposalContentRow.insertCell();
    let itemNameCell = proposalContentRow.insertCell();
    let actionsCell = proposalContentRow.insertCell();

    addProposalContentCellClasses(proposalContentIdCell);
    addProposalContentCellClasses(itemIdCell);
    addProposalContentCellClasses(itemNameCell);
    addProposalContentCellClasses(actionsCell);

    setProposalContentIdContent(proposalContentIdCell);
    setProposalContentItemIdContent(itemIdCell);
    setProposalContentItemNameContent(itemNameCell);
    setProposalContentActionContent(actionsCell);
}

function createNewProposalContentTableRow() {
    let rowId = 'proposalContentRow_' + NEW_PROPOSAL_CONTENT_ID;
    let row = proposalContentTable.insertRow();
    row.id = rowId;

    createProposalContentCells(row);

    return row;
}

function selectionChange() {
    let itemList = getItemList();
    if (itemList.value == 0) return;

    let proposalContentTable = getProposalContentTable();

    if (isItemInTable(itemList.value)) {
        alert('O item já existe na tabela!');
        setItemDefault();
        return;
    }

    createNewProposalContentTableRow();
    setItemDefault();
}

function isItemInTable(itemId) {
    let proposalContentTable = getProposalContentTable();

    for (let i = 0; i < proposalContentTable.rows.length; i++) {
        let row = proposalContentTable.rows[i];
        let rowItemId = row.cells[1].textContent; // A célula 1 contém o ID do item

        // Verifica se o ID do item já existe na tabela
        if (rowItemId == itemId) {
            return true;
        }
    }

    return false;
}

function deleteItemOnServer(proposalContentId) {
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Proposal/DeleteProposalContent/' + proposalContentId);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.onload = function () {
        if (xhr.status === 200) {
            alert('Item removido com sucesso!')
        } else {
            alert('Ocorreu um erro ao deletar o item!')
        }
    };
    xhr.send();
}

function deleteItem(proposalContentId, itemId) {
    if (confirm('Tem certeza de que deseja excluir o Proposal Content ID: ' + proposalContentId + ', Item ID: ' + itemId + '?')) {
        var row = document.getElementById('proposalContentRow_' + proposalContentId);
        if (row) {
            if (proposalContentId && proposalContentId != 0) {
                deleteItemOnServer(proposalContentId);
            }

            row.remove();
        }
    }
}