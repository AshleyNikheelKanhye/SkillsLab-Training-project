class ServerCall {
    url;
    parameters;
    callMethod;

    constructor(args) {
        this.url = args.url;
        this.parameters = args.parameters;
        this.callMethod = args.callMethod;
    }
    
    fetchApiCall() {
        return fetch(this.url, {
            method: this.callMethod,
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(this.parameters)
        })
            .then(response => { return response.json() })
            .catch((error) => console.log(error))
    }
}