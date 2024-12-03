export type Stop = {
  stopID: number;
  name: string;
  description: string;
  roomNr: string;
  divisionID: number;
  stopGroupID: number | null;
}

export type StopGroup = {
  stopGroupID: number;
  name: string;
  description: string;
  color: string;
}

export type StopGroupWithStops = {
  stopGroupID: number;
  name: string;
  description: string;
  color: string;
  stops: Stop[];
}
