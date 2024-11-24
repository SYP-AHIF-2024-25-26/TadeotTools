export type Stop = {
    stopID: number;
    name: string;
    description: string;
    roomNr: string;
}

export type ResponseStopGroup = {
    stopGroupID: number;
    name: string;
    description: string;
    color: string;
}

export type StopGroup = {
    stopGroupID: number;
    name: string;
    description: string;
    color: string;
    stops: Stop[];
}
