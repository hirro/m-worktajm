//
//  DetailViewController.h
//  Worktajm
//
//  Created by Jim Arnell on 10/12/14.
//  Copyright (c) 2014 Arnell Consulting AB. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface DetailViewController : UIViewController

@property (strong, nonatomic) id detailItem;
@property (weak, nonatomic) IBOutlet UILabel *detailDescriptionLabel;

@end

