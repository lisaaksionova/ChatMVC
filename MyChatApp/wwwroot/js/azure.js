const inputField = document.getElementById('message-input');
const submitBtn = document.getElementById('send-btn');

async function getSecrets() {
    try {
        const response = await fetch('/api/config/secrets');
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const secrets = await response.json();
        return secrets;
    } catch (error) {
        console.error('Error fetching secrets:', error);
    }
}

let endpoint = '';
let apiKey = '';

getSecrets().then(secrets => {
    if (secrets) {
        endpoint = secrets.endpoint; 
        apiKey = secrets.apiKey;     
    }
});


async function analyzeSentiment(text) {
    const response = await fetch(endpoint, {
        method: 'POST',
        headers: {
            'Ocp-Apim-Subscription-Key': apiKey,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            documents: [
                { id: "1", language: "en", text: text }
            ]
        })
    });

    const data = await response.json();
    return data.documents[0].sentiment;
}

function stopSentiment() {
    inputField.style.backgroundColor = " #fff";
    submitBtn.style.opacity = 1;
    submitBtn.style.cursor = "allowed";
}

function displaySentiment(sentiment) {
    switch (sentiment) {
        case "positive":
            inputField.style.backgroundColor = "#90EE90";
            submitBtn.style.opacity = 1;
            submitBtn.style.cursor = "pointer"; // Use "pointer" for clickable
            submitBtn.disabled = false; // Enable the button
            break;
        case "negative":
            inputField.style.backgroundColor = "#F08080";
            submitBtn.style.opacity = 0.6;
            submitBtn.style.cursor = "not-allowed"; // Not clickable
            submitBtn.disabled = true; // Disable the button
            break;
        case "neutral":
            inputField.style.backgroundColor = "#FFFFE0";
            submitBtn.style.opacity = 1;
            submitBtn.style.cursor = "pointer"; // Use "pointer" for clickable
            submitBtn.disabled = false; // Enable the button
            break;
        default:
            // Optional: Reset styles and enable button if sentiment is unrecognized
            inputField.style.backgroundColor = ""; // Reset to default
            submitBtn.style.opacity = 1;
            submitBtn.style.cursor = "pointer"; // Reset cursor
            submitBtn.disabled = false; // Enable the button
            break;
    }
}



let debounceTimer;

inputField.addEventListener('input', (event) => {
    clearTimeout(debounceTimer);
    debounceTimer = setTimeout(async () => {
        const message = event.target.value;
        if (message.length > 0) {
            const sentiment = await analyzeSentiment(message);
            displaySentiment(sentiment);
        } else {
            stopSentiment();
        }
    }, 300); 
});
