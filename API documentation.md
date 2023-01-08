---
title: MandelbrotAPI v1.0
language_tabs: []
language_clients: []
toc_footers: []
includes: []
search: true
highlight_theme: darkula
headingLevel: 2

---

<!-- Generator: Widdershins v4.0.1 -->

<h1 id="mandelbrotapi">MandelbrotAPI v1.0</h1>

> Scroll down for example requests and responses.

<h1 id="mandelbrotapi-diagnostic">Diagnostic</h1>

## Diagnosis

<a id="opIdDiagnosis"></a>

> Code samples

`GET /api/Diagnostic`

> Example responses

> 200 Response

```
0
```

```json
0
```

<h3 id="diagnosis-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|number|

<aside class="success">
This operation does not require authentication
</aside>

<h1 id="mandelbrotapi-mandelbrot">Mandelbrot</h1>

## MandelBrot

<a id="opIdMandelBrot?from={param1}&to={param2}&step={param3}&iter={param4}&split={param5}"></a>

> Code samples

`GET /Mandelbrot`

<h3 id="mandelbrot-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|from|query|string|false|none|
|to|query|string|false|none|
|step|query|number(double)|false|none|
|iter|query|integer(int32)|false|none|
|split|query|integer(int32)|false|none|

> Example responses

> 200 Response

```
{"from":{"real":0,"imaginary":0,"magnitude":0,"phase":0},"to":{"real":0,"imaginary":0,"magnitude":0,"phase":0},"points":[[0]]}
```

```json
{
  "from": {
    "real": 0,
    "imaginary": 0,
    "magnitude": 0,
    "phase": 0
  },
  "to": {
    "real": 0,
    "imaginary": 0,
    "magnitude": 0,
    "phase": 0
  },
  "points": [
    [
      0
    ]
  ]
}
```

<h3 id="mandelbrot-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[MandelbrotSet](#schemamandelbrotset)|

<aside class="success">
This operation does not require authentication
</aside>

<h1 id="mandelbrotapi-mandelbrotcomput">MandelbrotComput</h1>

## MandelBrot

<a id="opIdMandelBrot?off_i={param1}&off_j={param2}&from={param3}&to={param4}&step={param5}&iter={param6}"></a>

> Code samples

`GET /api/MandelbrotComput`

<h3 id="mandelbrot-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|off_i|query|integer(int32)|false|none|
|off_j|query|integer(int32)|false|none|
|from|query|string|false|none|
|to|query|string|false|none|
|step|query|number(double)|false|none|
|iter|query|integer(int32)|false|none|

> Example responses

> 200 Response

```
{"off_i":0,"off_j":0,"mbs":{"from":{"real":0,"imaginary":0,"magnitude":0,"phase":0},"to":{"real":0,"imaginary":0,"magnitude":0,"phase":0},"points":[[0]]}}
```

```json
{
  "off_i": 0,
  "off_j": 0,
  "mbs": {
    "from": {
      "real": 0,
      "imaginary": 0,
      "magnitude": 0,
      "phase": 0
    },
    "to": {
      "real": 0,
      "imaginary": 0,
      "magnitude": 0,
      "phase": 0
    },
    "points": [
      [
        0
      ]
    ]
  }
}
```

<h3 id="mandelbrot-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[MandelBrotPart](#schemamandelbrotpart)|

<aside class="success">
This operation does not require authentication
</aside>

# Schemas

<h2 id="tocS_Complex">Complex</h2>
<!-- backwards compatibility -->
<a id="schemacomplex"></a>
<a id="schema_Complex"></a>
<a id="tocScomplex"></a>
<a id="tocscomplex"></a>

```json
{
  "real": 0,
  "imaginary": 0,
  "magnitude": 0,
  "phase": 0
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|real|number(double)|false|none|none|
|imaginary|number(double)|false|none|none|
|magnitude|number(double)|false|read-only|none|
|phase|number(double)|false|read-only|none|

<h2 id="tocS_MandelBrotPart">MandelBrotPart</h2>
<!-- backwards compatibility -->
<a id="schemamandelbrotpart"></a>
<a id="schema_MandelBrotPart"></a>
<a id="tocSmandelbrotpart"></a>
<a id="tocsmandelbrotpart"></a>

```json
{
  "off_i": 0,
  "off_j": 0,
  "mbs": {
    "from": {
      "real": 0,
      "imaginary": 0,
      "magnitude": 0,
      "phase": 0
    },
    "to": {
      "real": 0,
      "imaginary": 0,
      "magnitude": 0,
      "phase": 0
    },
    "points": [
      [
        0
      ]
    ]
  }
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|off_i|integer(int32)|false|none|none|
|off_j|integer(int32)|false|none|none|
|mbs|[MandelbrotSet](#schemamandelbrotset)|false|none|none|

<h2 id="tocS_MandelbrotSet">MandelbrotSet</h2>
<!-- backwards compatibility -->
<a id="schemamandelbrotset"></a>
<a id="schema_MandelbrotSet"></a>
<a id="tocSmandelbrotset"></a>
<a id="tocsmandelbrotset"></a>

```json
{
  "from": {
    "real": 0,
    "imaginary": 0,
    "magnitude": 0,
    "phase": 0
  },
  "to": {
    "real": 0,
    "imaginary": 0,
    "magnitude": 0,
    "phase": 0
  },
  "points": [
    [
      0
    ]
  ]
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|from|[Complex](#schemacomplex)|false|none|none|
|to|[Complex](#schemacomplex)|false|none|none|
|points|[array]¦null|false|none|none|

