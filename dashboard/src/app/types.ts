export type Stop = {
    stopID: number;
    name: string;
    description: string;
    roomNr: string;
    stopGroupID: number | null;
    divisionID: number;
}

export type StopGroup = {
  stopGroupID: number;
  name: string;
  description: string;
  isPublic: boolean;
}

export type Division = {
  divisionID: number;
  name: string;
  color: string;
  image: File | null;
};
