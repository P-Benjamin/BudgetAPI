const API_BASE = "https://localhost:7058/api";
const token = localStorage.getItem("jwt");

window.addEventListener('DOMContentLoaded', () => {
    loadSources(); 
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

async function loadSources() {
    try {
        const response = await fetch("/api/sources", {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
            }
        });
        const sources = await response.json();

        const selectIncomeSource = document.getElementById("income-source");
        const selectOutcomeSource = document.getElementById("outcome-source");

        selectIncomeSource.innerHTML = "";
        selectOutcomeSource.innerHTML = "";

        const incomePlaceholder = document.createElement("option");
        incomePlaceholder.value = "";
        incomePlaceholder.textContent = "-- Choisir une source --";
        incomePlaceholder.disabled = true;
        incomePlaceholder.selected = true;

        const outcomePlaceholder = incomePlaceholder.cloneNode(true);

        selectIncomeSource.appendChild(incomePlaceholder);
        selectOutcomeSource.appendChild(outcomePlaceholder);

        sources.forEach(source => {
            const option1 = document.createElement("option");
            option1.value = source.id;
            option1.textContent = source.name;

            const option2 = option1.cloneNode(true); 

            selectIncomeSource.appendChild(option1);
            selectOutcomeSource.appendChild(option2);
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