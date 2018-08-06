import * as client from '../data/client';

let configuration;
async function umbracoConfiguration(){
    let response = await client.makeRequest('/umbraco/api/ConfigurationApi/GetConfiguration');
    configuration = response.data;
    return configuration;
}
export async function getConfiguration(){
    if(configuration){
        console.log("configuration already exist");
        return configuration;
    }
    else{
        await umbracoConfiguration();
        return configuration;
    }
}
