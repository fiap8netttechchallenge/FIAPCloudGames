# Configuração do Grafana para Monitoramento

Este guia descreve como configurar o Grafana para exibir métricas e logs utilizando as fontes de dados **Prometheus** e **Loki**.

---

## Configuração do Grafana:

- Primeiro, acesse o Grafana através do endereço `http://localhost:3000` (ou o endereço configurado no seu ambiente).

No menu lateral “Connections” vá em “Data Sources” e clique em Add data source

![Imagem 9](./monitoring/images/imagem9.png)

Selecione “Prometheus”

![Imagem 7](./monitoring/images/imagem7.png)

Em “Connection”, coloque a URL do servidor do Prometheus, no caso do servidor configurado no “docker-compose”, será `http://prometheus:9090`.
Em seguida clique em “Save & test” no fim da página.

![Imagem 3](./monitoring/images/imagem3.png)

Após configuração da conexão com Prometheus, clique em “Add new connection” ainda no menu lateral “Connections”, procure e clique em “Loki”.

![Imagem 1](./monitoring/images/imagem1.png)

Em seguida, clique no botão “Add new data source”.

![Imagem 10](./monitoring/images/imagem10.png)

Em “Connection”, coloque a URL do servidor do Loki, no caso do servidor configurado no “docker-compose”, será `http://loki:3100`.
Em seguida clique em “Save & test” no fim da página.

![Imagem 6](./monitoring/images/imagem6.png)

Feita a configuração dos Data sources, clique no menu lateral “Dashboards” e no canto superior direito clique em “New” e em seguida “Import”.

![Imagem 4](./monitoring/images/imagem4.png)

Neste ponto vamos importar um Dashboard que está incluso no diretório “monitoring” do repositório. Clique em “Upload dashboard JSON file” e selecione o arquivo “OpenTelemetry dotnet webapi  fiapCloudGame-1753534373730.json”

![Imagem 2](./monitoring/images/imagem2.png)

Em seguida, clique em “Import”.

![Imagem 11](./monitoring/images/imagem11.png)

A partir desse momento, durante o uso da aplicação os dados serão coletados e exibidos na Dashboard carregada.

![Imagem 8](./monitoring/images/imagem8.png)

![Imagem 5](./monitoring/images/imagem5.png)
