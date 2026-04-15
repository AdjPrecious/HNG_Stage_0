# HNG_Stage 0
# Gender Classification API

## 📌 Overview

This API provides gender classification for a given name using the Genderize API.
It processes the external API response and returns a structured, enriched result with confidence scoring.

---

## 🚀 Base URL

```
https://your-api-url.com
```

---

## 📍 Endpoint

### GET /api/classify

Classifies a name by gender.

### 🔹 Query Parameters

| Parameter | Type   | Required | Description      |
| --------- | ------ | -------- | ---------------- |
| name      | string | Yes      | Name to classify |

---

## ✅ Example Request

```
GET /api/classify?name=Precious
```

---

## ✅ Example Success Response

```json
{
  "status": "success",
  "data": {
    "name": "Precious",
    "gender": "female",
    "probability": 0.98,
    "sample_size": 1200,
    "is_confident": true,
    "processed_at": "2026-04-14T10:00:00Z"
  }
}
```

---

## ❌ Error Responses

### 400 Bad Request

```json
{
  "status": "error",
  "message": "Name query parameter is required"
}
```

### 422 Unprocessable Entity

```json
{
  "status": "error",
  "message": "Name must contain only alphabetic characters"
}
```

### 500 / 502 Server Errors

```json
{
  "status": "error",
  "message": "Internal server error"
}
```

### No Prediction Available

```json
{
  "status": "error",
  "message": "No prediction available for the provided name"
}
```

---

## ⚙️ Processing Logic

The API performs the following:

* Calls Genderize API
* Extracts:

  * gender
  * probability
  * count → renamed to **sample_size**
* Computes:

  * **is_confident = true** if:

    * probability ≥ 0.7 AND sample_size ≥ 100
* Adds:

  * **processed_at** (UTC, ISO 8601 format)

---

## ⚠️ Validation Rules

* Missing or empty `name` → **400 Bad Request**
* Non-alphabetic input → **422 Unprocessable Entity**

> Note: Query parameters are always strings in HTTP, so validation is based on content rather than type.

---

## 🌐 CORS

CORS is enabled:

```
Access-Control-Allow-Origin: *
```

---

## ⚡ Performance

* Response time < 500ms (excluding external API latency)
* Uses HttpClient for efficient request handling
* Supports concurrent requests

---

## 🛠️ Tech Stack

* ASP.NET Core Web API
* C#
* HttpClient
* Swagger (for testing)

---

## ▶️ Running Locally

1. Clone the repository:

```
git clone https://github.com/yourusername/gender-classifier-api.git
```

2. Navigate into the project:

```
cd gender-classifier-api
```

3. Run the application:

```
dotnet run
```

4. Open Swagger:

```
https://localhost:<port>/swagger
```

---

## 🌍 Deployment

Deployed at:

```
https://hng-stage-0-utkc.onrender.com/
```

---

## 🧪 Testing

Example:

```
https://your-api-url.com/api/classify?name=John
```

---

## 📄 License

This project is for educational and assessment purposes.
