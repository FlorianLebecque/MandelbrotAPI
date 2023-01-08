# Mandelbrot API
Scalable architecture project. The mandelbrot API is an ASP.NET Core API with vertical and horizontal scalability in mind. The API compute the mandelbrot set using multiple Workers and can use different server to have the work done.

## API Documentation
The API has three controllers and routes.

| Controller  | method | route | description  |
|------|---|---|---|
| Diagnostic | GET | /api/Diagnostic  | Return a string representing the %CPU utilization of the server |
| MandelbrotComput |GET | /api/MandelbrotComput?off_i={}&off_j={}&from={}&to={}&step={}&iter={}  | Call that comput a region of the mandelbrot set, this call is not pass in the load balancer  | 
| Mandelbrot | GET | /Mandelbrot?from={}&to={}&step={}&iter={}&split={} | The main API Call, this call is redirected througt the API load balancer |

A more extended documentation can be found on the git

## Load Balancer
The API can be verticaly and horizontaly scaled, It has been implemented in C# using the ASP.NET Core library.

When a user make an API Call to compute a region, the user call the `Mandelbrot` route. That route don't actualy compute anything. The region is computed using the `MandelbrotCompute` route and is only called by the load balancer.

The load balancer work by spliting the region that the user want to compute (using the `split` parameter). For every region we create a `QueueCall` object and we asign it a remote server (it can be `localhost`).

All the `QueueCall` objects create a `Task` object which is added to a thread pool and an job list. All the `Task` are executed on different threads, We just have to wait that all the `Task` status are set to terminated. After all the `QueueCall` have finished, we can merge the regions back to one and return the resutl to the user.

[![](https://mermaid.ink/img/pako:eNpVkE1ugzAQha8y8jq5AItWlJ-qEqnakKpSDAsXT8Aq2NSMpUaEu9dA1DZejKzx9-aN38gqI5EFrLaib-AQFxr8CflbnuwhfHmCKMyyErbbuwee960isFgro5cWRDyyKAjh1aHDSLQttGqgcp0SzcyRQ2o8VDV_0CqOx0wM9E96P626eHm-wLPxJeGp0hI-cJitO0O4qhXPSViCgxg-y6tA4zfB5Qi3Y844-Jryd-G3n32-ZsfrjukCPfId2hpv_tbwPZKzumQb1qHthJI-p3GWFYwa7LBggb9KPAnXUsEKPXlUODL5WVcsIOtww1wvfUCxEj7hjgUn0Q6_3UQqMnYlpx85v31j?type=png)](https://mermaid.live/edit#pako:eNpVkE1ugzAQha8y8jq5AItWlJ-qEqnakKpSDAsXT8Aq2NSMpUaEu9dA1DZejKzx9-aN38gqI5EFrLaib-AQFxr8CflbnuwhfHmCKMyyErbbuwee960isFgro5cWRDyyKAjh1aHDSLQttGqgcp0SzcyRQ2o8VDV_0CqOx0wM9E96P626eHm-wLPxJeGp0hI-cJitO0O4qhXPSViCgxg-y6tA4zfB5Qi3Y844-Jryd-G3n32-ZsfrjukCPfId2hpv_tbwPZKzumQb1qHthJI-p3GWFYwa7LBggb9KPAnXUsEKPXlUODL5WVcsIOtww1wvfUCxEj7hjgUn0Q6_3UQqMnYlpx85v31j)

### Verticaly
The `Task` object is an built-in wrapper in C# allowing the developper to use paralellisme using simple `async` and `await` keyworks.
`Task` in C# works by being added to a job queue and are dispatch on a thread pool, the thread marks the `Task` done and wait to be asigned a new `Task`

### Asigning QueueCall remote
As stated the API can scale horizontaly. In a single server mode, the load balancer asigne `localhost` as the `QueueCall` remote.
We can specify other remotes servers that the load balancer can use.

The load balancer asign the remote server that has the least ping and less than 70% CPU utilization. If no remotes servers satisfy this condition, we select the first one.

### Smart Load balancer
The load balancer is considered smart because it take decision based on the status of the servers. To be able to do that, the API need a way to read the %CPU utilization of the server. It sound pretty simple but it is actualy a very long process (at least 1s to be accurate).

The load balancer also need to be faster than computing the required task. The issues is solved in two part.
- `PerformanceThread`
- `RemoteAnalyzer`

#### The PerformanceThread
The `PerformanceThread` is a thread that compute the %CPU utilization in continious. the `Diagnostic` route simply lock the thread and read the value when needed. From taking more than 1s to less then a ns.

[![](https://mermaid.ink/img/pako:eNp9ks9vgjAUx_-Vl55ly64kmiDOacY2gxAPhUNnn0KE4tpyMMj_vlLmYB72Di39fN-vlteQfcWRuOQo2TmDaJEIMKbqzx74ldCyKnp6osGH__oYv3db2jMUPBF3Qd5mDb4XBD3u7IUucnYUldL5HkL8qlHpdCSD48wgsOlTcB4cOA1iYMWQSmR8FBNaHNOfbu6jYitHNERdS_FPs1HWJR4CPbrVTGrQ2V1BD6bTGcybSNbYDnhusW8KMQ7-Jh5F-FZaUFUgnuFJpfa8bNYKup7bWy-dLa12BVGZZUV3Mtc4SrXqi4_9u_td4YLKrGu6Y7lOLVv-cRpeZXT5204mpERZspybCWg6lhCdYYkJcc0nxwOrC52QRLTGldW62l7EnrjaPMGE1GfONHY_VrKSuAdWqF_6zHNdyZsn2tNbP2l24NpvgJWzCQ?type=png)](https://mermaid.live/edit#pako:eNp9ks9vgjAUx_-Vl55ly64kmiDOacY2gxAPhUNnn0KE4tpyMMj_vlLmYB72Di39fN-vlteQfcWRuOQo2TmDaJEIMKbqzx74ldCyKnp6osGH__oYv3db2jMUPBF3Qd5mDb4XBD3u7IUucnYUldL5HkL8qlHpdCSD48wgsOlTcB4cOA1iYMWQSmR8FBNaHNOfbu6jYitHNERdS_FPs1HWJR4CPbrVTGrQ2V1BD6bTGcybSNbYDnhusW8KMQ7-Jh5F-FZaUFUgnuFJpfa8bNYKup7bWy-dLa12BVGZZUV3Mtc4SrXqi4_9u_td4YLKrGu6Y7lOLVv-cRpeZXT5204mpERZspybCWg6lhCdYYkJcc0nxwOrC52QRLTGldW62l7EnrjaPMGE1GfONHY_VrKSuAdWqF_6zHNdyZsn2tNbP2l24NpvgJWzCQ)

#### The RemoteAnalyzer
The `RemoteAnalyzer` works on the same principle as the `PerformanceThread`. It ask every 5s its remotes for a diagnostic and store the %CPU utilization and the time it took to obtain that value.

The load balancer then lock the thread to read the status of all remotes. This method allow to have an insight of the remotes servers status and helps choosing which remotes servers to use.

## Class diagram
[![](https://mermaid.ink/img/pako:eNqtV1Fv0zAQ_itWnjpIJBiChzKQWAcIxCTYJiFBUeQm19bgxMFxYGPsv3OO0_ScuFsR9KXx5fPdd9-dz-11lKkcommUSV7XJ4KvNC_mJcNPa2GnvMxBHmtl3nNtrt0r-7nPRGmYWi5TETJ-pUbnZIFOzsGwYlHPy-1rP8Kk9xr3ruKZKioJl2ypVRGzzcqomOWqWUhgtYEqbmMLA_rgr5yPyJHtteFGZOwU9Aom70RtJr7DAybR6CL_FLlZu8c1iNW6M9eVFIib2sXn-IvzfbMRYCgyUvA0ppmH7BeKWg9PUoySWk6sUvhYb98mHQFW-Vai4ADMCn6ZWj0DcI26qHEVOxUn-xVsZ72CbqiXcaLER6XFD26AZbihMTAZik8xb-o3pYvZB_sUe8l3uze5Dir3HvRS6YKXGVysNfD8morlTMzofGw9g7qRyB4d0JfE4Uw1JRJoIWnmFk-p5pskZi5RsnVC5UDBsYdfg_EAmNZSKm6CaVGKXj-2W9gPLhsIbvzQQAMzLuX1sJkGkyIJTYqkP8LU1h3CtTHVTApAhPuiUvRxyRHfRojZ_5ogG62pwGfwvQEcDbXRolwxUVltL3j97cjf-7xXzH5R1c6gUAZelFxe_QLtSWeHTuredx3jFne32Ug2qpcfsR1tR47_c6bbd6ET5QK9asoMT39JNcDuoiTb9joRLYzrq9S59vIItg8FBPquQidja1Y11GgpqlJesa4ea1UHUu94EsxBkNE7xXMsI18N6rJQSrKvalGnGAxGFbPlTwetY9EjYN-47Lt98gBekXDk0iwIr7uqN0NFDLzF6JP-Qop3jdY9jgPGtNFp-c-NvV2J4SMXZtQfreI7KOw6k7aRvOvxlgvUXhszVRqtpPTKhdE3lbbB2DM2j5JH0-RwHsWbNjGqNR9OWyNhgObHDx70lxUuH7lVmwUuHz7ZklzsSdIOkQHB8OD6V9rM4-2rabsyyNT-DizxSIhsp5y77o_QVnb0O0nYdn3Ma-jm31CTPaF3eL6Fyr0kGV_aDj8ys8S_rDeOR6QRN1Q1hPVIkAPskMTQAvwRPcb0EDpP_U1hTJBSIIX9sG23U3A_7-7twm1nXiBqFEcFYBVEjv9K2qabR2YNBcyjKT7msOQ2k2he3iCUN0adX5VZNF1yWUMcNVWOw677IzOwvsyFUTqaGt2gDdrVaff3x37d_AEhfDbx?type=png)](https://mermaid.live/edit#pako:eNqtV1Fv0zAQ_itWnjpIJBiChzKQWAcIxCTYJiFBUeQm19bgxMFxYGPsv3OO0_ScuFsR9KXx5fPdd9-dz-11lKkcommUSV7XJ4KvNC_mJcNPa2GnvMxBHmtl3nNtrt0r-7nPRGmYWi5TETJ-pUbnZIFOzsGwYlHPy-1rP8Kk9xr3ruKZKioJl2ypVRGzzcqomOWqWUhgtYEqbmMLA_rgr5yPyJHtteFGZOwU9Aom70RtJr7DAybR6CL_FLlZu8c1iNW6M9eVFIib2sXn-IvzfbMRYCgyUvA0ppmH7BeKWg9PUoySWk6sUvhYb98mHQFW-Vai4ADMCn6ZWj0DcI26qHEVOxUn-xVsZ72CbqiXcaLER6XFD26AZbihMTAZik8xb-o3pYvZB_sUe8l3uze5Dir3HvRS6YKXGVysNfD8morlTMzofGw9g7qRyB4d0JfE4Uw1JRJoIWnmFk-p5pskZi5RsnVC5UDBsYdfg_EAmNZSKm6CaVGKXj-2W9gPLhsIbvzQQAMzLuX1sJkGkyIJTYqkP8LU1h3CtTHVTApAhPuiUvRxyRHfRojZ_5ogG62pwGfwvQEcDbXRolwxUVltL3j97cjf-7xXzH5R1c6gUAZelFxe_QLtSWeHTuredx3jFne32Ug2qpcfsR1tR47_c6bbd6ET5QK9asoMT39JNcDuoiTb9joRLYzrq9S59vIItg8FBPquQidja1Y11GgpqlJesa4ea1UHUu94EsxBkNE7xXMsI18N6rJQSrKvalGnGAxGFbPlTwetY9EjYN-47Lt98gBekXDk0iwIr7uqN0NFDLzF6JP-Qop3jdY9jgPGtNFp-c-NvV2J4SMXZtQfreI7KOw6k7aRvOvxlgvUXhszVRqtpPTKhdE3lbbB2DM2j5JH0-RwHsWbNjGqNR9OWyNhgObHDx70lxUuH7lVmwUuHz7ZklzsSdIOkQHB8OD6V9rM4-2rabsyyNT-DizxSIhsp5y77o_QVnb0O0nYdn3Ma-jm31CTPaF3eL6Fyr0kGV_aDj8ys8S_rDeOR6QRN1Q1hPVIkAPskMTQAvwRPcb0EDpP_U1hTJBSIIX9sG23U3A_7-7twm1nXiBqFEcFYBVEjv9K2qabR2YNBcyjKT7msOQ2k2he3iCUN0adX5VZNF1yWUMcNVWOw677IzOwvsyFUTqaGt2gDdrVaff3x37d_AEhfDbx)