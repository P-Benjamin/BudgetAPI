const API_BASE = "https://localhost:7058/api";
const token = localStorage.getItem("jwt");

window.addEventListener('DOMContentLoaded', () => {
    loadSources(); 
    fetchSources();
    fetchIncomes();
    fetchOutcomes();
});

async function getTotal(type) {
    const res = await fetch(`${API_BASE}/${type}/total`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        }
    });
    const data = await res.text();
    document.getElementById("total-result").textContent = `${type}: ${data}`;
}

async function getTotalByMonth(type) {
    const year = document.getElementById("year-month").value;
    const month = document.getElementById("month").value;
    const res = await fetch(`${API_BASE}/${type}/total/month/${year}/${month}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        }
    });
    const data = await res.text();
    document.getElementById("month-result").textContent = `${type} (${month}/${year}): ${data}`;
}

async function getTotalByYear(type) {
    const year = document.getElementById("year-total").value;
    const res = await fetch(`${API_BASE}/${type}/total/year/${year}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        }
    });
    const data = await res.text();
    document.getElementById("year-result").textContent = `${type} (${year}): ${data}`;
}

async function getTotalByRange(type) {
    const start = document.getElementById("start-date").value;
    const end = document.getElementById("end-date").value;
    const res = await fetch(`${API_BASE}/${type}/total/range`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json", 'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify({ start, end })
    });
    const data = await res.text();
    document.getElementById("range-result").textContent = `${type} (${start} → ${end}): ${data}`;
}

async function fetchIncomes() {
    const res = await fetch(`${API_BASE}/incomes`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        }
    })
    const data = await res.json();
    const tbody = document.querySelector("#income-table tbody");
    tbody.innerHTML = "";
    data.forEach(i => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
                                            <td>${i.id}</td>
                                            <td>${i.sourceName}</td>
                                            <td>${i.amount}</td>
                                            <td>${i.dateReceived.split("T")[0]}</td>
                                            <td>
                                                <button onclick="editIncome(${i.id}, '${i.sourceId}', ${i.amount}, '${i.dateReceived.split("T")[0]}')">Modifier</button>
                                                <button onclick="deleteIncome(${i.id})">Supprimer</button>
                                            </td>
                                        `;
        tbody.appendChild(tr);
    });
}

async function fetchIncomesBySource() {
    const sourceId = document.getElementById("source-filter").value;
    if (!sourceId) {
        alert("Veuillez choisir une source.");
        return;
    }

    const response = await fetch(`/api/incomes/by-source/${sourceId}`, {
        headers: { "Authorization": `Bearer ${token}` }
    });

    if (!response.ok) {
        alert("Erreur lors de la récupération des données.");
        return;
    }

    const incomes = await response.json();
    const tbody = document.querySelector("#filtered-income-table tbody");
    tbody.innerHTML = "";

    incomes.forEach(income => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${income.id}</td>
            <td>${income.sourceName}</td>
            <td>${income.amount.toFixed(2)}</td>
            <td>${new Date(income.dateReceived).toLocaleDateString()}</td>
        `;
        tbody.appendChild(tr);
    });
}

function editIncome(id, source, amount, date) {
    document.getElementById("income-id").value = id;
    document.getElementById("income-source").value = source;
    document.getElementById("income-amount").value = amount;
    document.getElementById("income-date").value = date;
}

async function deleteIncome(id) {
    if (!confirm("Supprimer cet income ?")) return;
    await fetch(`${API_BASE}/incomes/${id}`, {
        method: "DELETE",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        }
    });
    fetchIncomes();
}

async function saveIncome() {
    const id = document.getElementById("income-id").value;
    const sourceId = parseInt(document.getElementById("income-source").value);
    const amount = parseFloat(document.getElementById("income-amount").value);
    const dateReceived = document.getElementById("income-date").value;

    const income = { sourceId, amount, dateReceived };

    console.log(income);

    if (!id) {
        await fetch(`${API_BASE}/incomes`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(income)
        });

    } else {
        await fetch(`${API_BASE}/incomes/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json", 'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({ id: parseInt(id), sourceId, amount, dateReceived })
        });
    }
    fetchIncomes();
}

async function fetchOutcomes() {
    const res = await fetch(`${API_BASE}/outcomes`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        }
    });
    const data = await res.json();
    const tbody = document.querySelector("#outcome-table tbody");
    tbody.innerHTML = "";
    data.forEach(i => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
                                <td>${i.id}</td>
                                <td>${i.sourceName}</td>
                                <td>${i.amount}</td>
                                <td>${i.dateReceived.split("T")[0]}</td>
                                <td>
                                    <button onclick="editOutcome(${i.id}, '${i.sourceId}', ${i.amount}, '${i.dateReceived.split("T")[0]}')">Modifier</button>
                                    <button onclick="deleteOutcome(${i.id})">Supprimer</button>
                                </td>
                            `;
        tbody.appendChild(tr);
    });
}

async function fetchOutcomesBySource() {
    const sourceId = document.getElementById("source-filter-outcome").value;
    if (!sourceId) {
        alert("Veuillez choisir une source.");
        return;
    }

    const response = await fetch(`/api/outcomes/by-source/${sourceId}`, {
        headers: { "Authorization": `Bearer ${token}` }
    });

    if (!response.ok) {
        alert("Erreur lors de la récupération des outcomes.");
        return;
    }

    const outcomes = await response.json();
    const tbody = document.querySelector("#filtered-outcome-table tbody");
    tbody.innerHTML = "";

    outcomes.forEach(outcome => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${outcome.id}</td>
            <td>${outcome.sourceName}</td>
            <td>${outcome.amount.toFixed(2)}</td>
            <td>${new Date(outcome.dateReceived).toLocaleDateString()}</td>
        `;
        tbody.appendChild(tr);
    });
}


async function loadSources() {
    try {
        const response = await fetch("/api/sources", {
            headers: { "Authorization": `Bearer ${token}` }
        });
        const sources = await response.json();

        const selectIncomeSource = document.getElementById("income-source");
        const selectOutcomeSource = document.getElementById("outcome-source");
        const selectFilterSource = document.getElementById("source-filter");
        const selectFilterSourceOutcome = document.getElementById("source-filter-outcome");


        [selectIncomeSource, selectOutcomeSource, selectFilterSource, selectFilterSourceOutcome].forEach(select => {
            select.innerHTML = "";
            const placeholder = document.createElement("option");
            placeholder.textContent = "-- Choisir une source --";
            placeholder.disabled = true;
            placeholder.selected = true;
            placeholder.value = "";
            select.appendChild(placeholder);
        });

        sources.forEach(source => {
            const option = document.createElement("option");
            option.value = source.id;
            option.textContent = source.name;

            selectIncomeSource.appendChild(option.cloneNode(true));
            selectOutcomeSource.appendChild(option.cloneNode(true));
            selectFilterSource.appendChild(option.cloneNode(true));
            selectFilterSourceOutcome.appendChild(option.cloneNode(true));

        });
    } catch (err) {
        console.error("Erreur lors du chargement des sources :", err);
    }
}

function editOutcome(id, source, amount, date) {
    document.getElementById("outcome-id").value = id;
    document.getElementById("outcome-source").value = source;
    document.getElementById("outcome-amount").value = amount;
    document.getElementById("outcome-date").value = date;
}

async function deleteOutcome(id) {
    if (!confirm("Supprimer cet outcome ?")) return;
    await fetch(`${API_BASE}/outcomes/${id}`, {
        method: "DELETE",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        }
    });
    fetchOutcomes();
}

async function saveOutcome() {
    const id = document.getElementById("outcome-id").value;
    const sourceId = parseInt(document.getElementById("outcome-source").value);
    const amount = parseFloat(document.getElementById("outcome-amount").value);
    const dateReceived = document.getElementById("outcome-date").value;

    const outcome = { sourceId, amount, dateReceived };

    if (!id) {
        await fetch(`${API_BASE}/outcomes`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Authorization': `Bearer ${token}`,
            },
            body: JSON.stringify(outcome)
        });
    } else {
        await fetch(`${API_BASE}/outcomes/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                'Authorization': `Bearer ${token}`,
            },
            body: JSON.stringify({ id: parseInt(id), sourceId, amount, dateReceived })
        });
    }
    fetchOutcomes();
}

async function fetchSources() {
    const response = await fetch("/api/sources", {
        headers: { "Authorization": `Bearer ${token}` }
    });
    const sources = await response.json();

    const tbody = document.querySelector("#source-table tbody");
    tbody.innerHTML = "";

    sources.forEach(s => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${s.id}</td>
            <td>${s.name}</td>
            <td>
                <button onclick="editSource(${s.id}, '${s.name}')">Modifier</button>
                <button onclick="deleteSource(${s.id})">Supprimer</button>
            </td>`;
        tbody.appendChild(tr);
    });
}

function editSource(id, name) {
    document.getElementById("source-id").value = id;
    document.getElementById("source-name").value = name;
}

async function saveSource() {
    const idValue = document.getElementById("source-id").value;
    const name = document.getElementById("source-name").value;

    const source = { name };

    const isEdit = idValue !== "";
    const url = isEdit ? `/api/sources/${idValue}` : "/api/sources";
    const method = isEdit ? "PUT" : "POST";

    if (isEdit) {
        source.id = parseInt(idValue);
    }

    const response = await fetch(url, {
        method,
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify(source)
    });

    if (!response.ok) {
        const error = await response.json();
        console.error("Erreur :", error);
        alert("Erreur lors de l'enregistrement.");
        return;
    }

    document.getElementById("source-id").value = "";
    document.getElementById("source-name").value = "";

    fetchSources();
    loadSources(); 
}

async function deleteSource(id) {
    if (!confirm("Supprimer cette source ?")) return;

    const response = await fetch(`/api/sources/${id}`, {
        method: "DELETE",
        headers: { "Authorization": `Bearer ${token}` }
    });

    if (response.status === 400) {
        const message = await response.text();
        alert("Erreur : " + message);
    }

    fetchSources();
    loadSources();
}

let chart;

async function loadChartData() {
    const year = 2025;
    const labels = Array.from({ length: 12 }, (_, i) => `${i + 1}`); // Mois 1 à 12

    const incomeData = await Promise.all(labels.map(async month => {
        const res = await fetch(`${API_BASE}/incomes/total/month/${year}/${month}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
            }
        });
        return parseFloat(await res.text());
    }));

    const outcomeData = await Promise.all(labels.map(async month => {
        const res = await fetch(`${API_BASE}/outcomes/total/month/${year}/${month}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
            }
        });
        return parseFloat(await res.text());
    }));

    const ctx = document.getElementById('budgetChart').getContext('2d');

    if (chart) chart.destroy(); // Pour éviter les duplications

    chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels.map(m => `M${m}`),
            datasets: [
                {
                    label: 'Incomes',
                    data: incomeData,
                    borderColor: 'green',
                    fill: false,
                    tension: 0.1
                },
                {
                    label: 'Outcomes',
                    data: outcomeData,
                    borderColor: 'red',
                    fill: false,
                    tension: 0.1
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                },
                title: {
                    display: true,
                    text: 'Incomes vs Outcomes - Année 2025'
                }
            }
        }
    });
}