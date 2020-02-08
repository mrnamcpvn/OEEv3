import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
  {
    name: 'Dashboard',
    url: '/dashboard',
    icon: 'icon-speedometer',
  },
  {
    name: 'Trend',
    url: '/trend',
    icon: 'icon-graph'
  },
  {
    name: 'Down time reasons',
    url: '/downtime-reasons',
    icon: 'icon-clock'
  },
  {
    name: 'Down time analysis',
    url: '/downtime-analysis',
    icon: 'icon-chart'
  }
];
