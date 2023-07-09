const NEW_PROPOSAL_CONTENT_ID = 0;
const NEW_PROPOSAL_CONTENT_NAME = 'proposalContentRow_';
const FULL_ID_NEW_PROPOSAL_CONTENT_ROW = NEW_PROPOSAL_CONTENT_NAME + NEW_PROPOSAL_CONTENT_ID;

function getDiscountElement() {
  return document.getElementById('discount');
}

function getItemList() {
  return document.getElementById('ItemList');
}

function setItemDefault() {
  let itemList = getItemList();
  itemList.value = 0;
}

function getProposalContentTable() {
  return document.getElementById('proposalContentTable');
}

function getTableRow() {
  return document.getElementById('table-rows');
}

function getActualRow() {
  return getTableRow().rows.length - 2; // nesse momento precisa ser -2 pois a ultima row é esse registro
}

function addProposalContentCellClasses(cell) {
  cell.classList.add('text-center');
}

function setProposalContentIdContent(proposalContentIdCell) {
  let inputProposalContentId =
    '<input type="hidden" name="ProposalContent[' + getActualRow() + '].ProposalContentId" value="' + NEW_PROPOSAL_CONTENT_ID + '" />';
  let inputITemId = '<input type="hidden" name="ProposalContent[' + getActualRow() + '].ItemId" value="' + getItemList().value + '" />';

  let span = '<span>' + NEW_PROPOSAL_CONTENT_ID + '</span>';

  proposalContentIdCell.innerHTML = inputProposalContentId + inputITemId + span;
}

function setProposalContentItemIdContent(itemIdCell) {
  let itemList = getItemList();
  itemIdCell.textContent = itemList.value;
}

function setProposalContentItemNameContent(itemNameCell) {
  let itemList = getItemList();
  itemNameCell.textContent = itemList.options[itemList.selectedIndex].text;
}

function setProposalContentValueContent(valueCell) {
  let itemId = getItemList().value;
  let item = items.find((i) => i.ItemId == itemId);

  if (item) {
    valueCell.textContent = 'R$ ' + parseFloat(item.Value).toFixed(2);
  } else {
    valueCell.textContent = 'error';
  }
  valueCell.id = 'item-value';
}
function setProposalContentQuantityContent(quantityCell) {
  let input = document.createElement('input');
  addProposalContentCellClasses(input);
  input.id = 'item-quantity';
  input.value = '1';
  input.name = 'ProposalContent[' + getActualRow() + '].Quantity';
  input.addEventListener('change', function () {
    calculateTotalValue();
  });

  quantityCell.appendChild(input);
}

function setProposalContentActionContent(actionsCell) {
  let itemList = getItemList();
  actionsCell.innerHTML =
    '<span class="delete-icon" onclick="deleteItem(' +
    NEW_PROPOSAL_CONTENT_ID +
    ', ' +
    itemList.value +
    ')">' +
    '<svg xmlns="http://www.w3.org/2000/svg" height="1em" viewBox="0 0 448 512">' +
    '<style>svg {fill: #e60a0a}</style>' +
    '<path d="M432 32H312l-9.4-18.7A24 24 0 0 0 281.1 0H166.8a23.72 23.72 0 0 0-21.4 13.3L136 32H16A16 16 0 0 0 0 48v32a16 16 0 0 0 16 16h416a16 16 0 0 0 16-16V48a16 16 0 0 0-16-16zM53.2 467a48 48 0 0 0 47.9 45h245.8a48 48 0 0 0 47.9-45L416 128H32z"/>' +
    '</svg>' +
    '</span>';
}

function createProposalContentCells(proposalContentRow) {
  let proposalContentIdCell = proposalContentRow.insertCell();
  let itemIdCell = proposalContentRow.insertCell();
  let itemNameCell = proposalContentRow.insertCell();
  let valueCell = proposalContentRow.insertCell();
  let quantityCell = proposalContentRow.insertCell();
  let actionsCell = proposalContentRow.insertCell();

  addProposalContentCellClasses(proposalContentIdCell);
  addProposalContentCellClasses(itemIdCell);
  addProposalContentCellClasses(itemNameCell);
  addProposalContentCellClasses(quantityCell);
  addProposalContentCellClasses(valueCell);
  addProposalContentCellClasses(actionsCell);

  setProposalContentIdContent(proposalContentIdCell);
  setProposalContentItemIdContent(itemIdCell);
  setProposalContentItemNameContent(itemNameCell);
  setProposalContentQuantityContent(quantityCell);
  setProposalContentValueContent(valueCell);
  setProposalContentActionContent(actionsCell);

  calculateTotalValue();
}

function getNewRowPosition() {
  const NEW_ROW_POSITION = proposalContentTable.rows.length - 1;
  return NEW_ROW_POSITION == 0 ? 1 : NEW_ROW_POSITION;
}

function createNewProposalContentTableRow() {
  let rowId = FULL_ID_NEW_PROPOSAL_CONTENT_ROW;
  let row = proposalContentTable.insertRow(getNewRowPosition());
  row.id = rowId;

  createProposalContentCells(row);

  return row;
}

function selectionChange() {
  let itemList = getItemList();
  if (itemList.value == 0) return;

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
      alert('Item removido com sucesso!');
    } else {
      alert('Ocorreu um erro ao deletar o item!');
    }
  };
  xhr.send();
}

function calculateTotalValue() {
  let proposalContentTable = getProposalContentTable();
  let totalValueCell = document.getElementById('total-value-cell');
  let totalValue = 0;

  for (let i = 0; i < proposalContentTable.rows.length; i++) {
    let row = proposalContentTable.rows[i];
    let itemValueCell = row.querySelector('#item-value');
    if (itemValueCell) {
      let itemValue = parseFloat(itemValueCell.textContent.replace('R$ ', ''));

      let itemQuantityCell = row.querySelector('#item-quantity');
      if (itemQuantityCell) {
        itemValue *= itemQuantityCell.value;
      }

      totalValue += itemValue;
    }
  }

  totalValue -= getDiscountElement().value;
  totalValueCell.textContent = 'R$ ' + totalValue.toFixed(2);
}

function deleteItem(proposalContentId, itemId) {
  if (confirm('Tem certeza de que deseja excluir o Proposal Content ID: ' + proposalContentId + ', Item ID: ' + itemId + '?')) {
    var row = document.getElementById(NEW_PROPOSAL_CONTENT_NAME + proposalContentId);
    if (row) {
      if (proposalContentId && proposalContentId != 0) {
        deleteItemOnServer(proposalContentId);
      }
      row.remove();
      calculateTotalValue();
    }
  }
}

function setPerson() {
  if (typeof selectPersonId === 'undefined' || selectPersonId === null) return;

  let personList = document.getElementById('PersonId');
  personList.value = selectPersonId;
}

function setDiscount() {
  calculateTotalValue();
}

document.addEventListener('DOMContentLoaded', () => {
  setPerson();
  setDiscount();
});

document.addEventListener('submit', () => {
  sortNewProposalContentId();
});

function onChangeItemQuantity(e) {
  if (e) {
    if (e.value <= 0) {
      e.value = 1;
    }
  }
}

function getExistentDBItems() {
  const tableRow = getTableRow();
  const trElements = tableRow.getElementsByTagName('tr');

  const existentDBItems = Array.from(trElements).filter((tr) => {
    const trID = tr.getAttribute('id');
    return trID && trID != FULL_ID_NEW_PROPOSAL_CONTENT_ROW;
  });

  return existentDBItems;
}

function updateProposalContentIndex(element, index) {
  const findIntValueRegex = /\d+/;
  element.name = element.name.replace(findIntValueRegex, index);
}

function sortNewProposalContentId() {
  const tableRowChildren = getTableRow().children;

  let existentDBItems = getExistentDBItems();
  sortDBProposalContentId(existentDBItems);

  let index = existentDBItems.length;

  Array.from(tableRowChildren).forEach((element) => {
    if (element.id == FULL_ID_NEW_PROPOSAL_CONTENT_ROW) {
      applyIndexForInputElements(element, index);

      index++;
    }
  });
}

function sortDBProposalContentId(existentDBItems) {
  let index = 0;
  Array.from(existentDBItems).forEach((element) => {
    applyIndexForInputElements(element, index);
  });
}

function applyIndexForInputElements(element, index) {
  let inputElements = element.getElementsByTagName('input');
  Array.from(inputElements).forEach((inputElement) => {
    updateProposalContentIndex(inputElement, index);
  });
}
