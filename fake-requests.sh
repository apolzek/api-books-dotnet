for i in {0..5}
do
    PESSOA=$(curl --silent 'https://www.4devs.com.br/ferramentas_online.php' -H 'authority: www.4devs.com.br' -H 'content-type: application/x-www-form-urlencoded' -H 'accept: */*' -H 'origin: https://www.4devs.com.br' -H 'referer: https://www.4devs.com.br/gerador_de_pessoas' -H 'accept-language: en-US,en;q=0.9,pt;q=0.8' --data-raw 'acao=gerar_pessoa&sexo=I&pontuacao=S&idade=0&cep_estado=&txt_qtde=1&cep_cidade=' --compressed)
    NOME=$(echo $PESSOA | jq .nome)
    # echo $NOME
    curl -X POST "http://localhost:4000/api/Books" -H  "accept: text/plain" -H  "Content-Type: application/json-patch+json" -d "{\"_id\":\"\",\"BookName\":\"Clean Code\",\"Price\":43.15,\"Category\":\"Computers\",\"Author\": $NOME}"
done;
