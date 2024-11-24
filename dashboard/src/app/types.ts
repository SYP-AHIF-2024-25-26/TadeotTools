export type Stop = {
    stopID: number;
    name: string;
    description: string;
    roomNr: string;
    stopGroupID: number | null;
}

export type StopGroup = {
    stopGroupID: number;
    name: string;
    description: string;
    color: string;
}
