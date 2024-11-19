export type GuideCard = {
    id: number;
    title: string;
    description: string;
  }

  export type Stop = {
    stopID: number;
    name: string;
    description: string;
    roomNr: string;
    stopGroupID: number;
    stopGroup: StopGroup;
  }

  export type StopGroup = {
    stopGroupID: number;
    name: number;
    description: string;
  }

  export type Division = {
    divisionID: number;
    name: string;
    color: string;
  }
