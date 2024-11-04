import { StopGroupName } from './stop-group-name.enum';

export type GuideCard = {
    id: number;
    title: string;
    description: string;
    isTour: boolean;
    stopGroupNames?: StopGroupName[];
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
    color: string;
  }
