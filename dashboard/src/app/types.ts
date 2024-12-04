export type Stop = {
  stopID: number;
  name: string;
  description: string;
  roomNr: string;
  divisionID: number;
  stopGroupID: number | null;
};

export type StopGroup = {
  stopGroupID: number;
  name: string;
  description: string;
  isPublic: boolean;
};

export type StopGroupWithStops = {
  stopGroupID: number;
  name: string;
  description: string;
  isPublic: boolean;
  stops: Stop[];
};

export type Division = {
  divisionID: number;
  name: string;
  color: string;
};
