# Разработать метод GET api/v1/isvpn

В теле запроса передавать ip пользователя для дальнейшего сопоставления среди ip из списков:

[https://github.com/X4BNet/lists_vpn/blob/main/output/vpn/ipv4.txt](https://github.com/X4BNet/lists_vpn/blob/main/output/vpn/ipv4.txt)

[datacenters](https://github.com/X4BNet/lists_vpn/blob/main/output/datacenter/ipv4.txt)

[bots](https://github.com/stamparm/ipsum/blob/master/ipsum.txt)

[https://github.com/josephrocca/is-vpn/blob/main/vpn-or-datacenter-ipv4-ranges.txt](https://github.com/josephrocca/is-vpn/blob/main/vpn-or-datacenter-ipv4-ranges.txt)

[TOR nodes - note that Cloudflare allows detecting TOR on free plan](https://github.com/X4BNet/lists_torexit/blob/main/ipv4.txt)

Проводить проверку каждый раз, когда пользователь загружает страницу.

Примерный ответ:

```json
{
  "ip": "127.0.0.1",
  "isVpn": false
}
```
