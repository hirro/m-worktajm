//
//  RootViewController.swift
//  worktajm
//
//  Created by Jim Arnell on 2015-01-05.
//  Copyright (c) 2015 Arnell Consulting AB. All rights reserved.
//

import UIKit

class RootViewController: UIViewController {
    
    private var loginViewController: LoginViewController!
    private var worktajmViewController: UITabBarController!
    
    override func viewDidLoad() {
        super.viewDidLoad()

        
        // Do any additional setup after loading the view.
        loginViewController = storyboard?.instantiateViewControllerWithIdentifier("Login")
            as LoginViewController
        loginViewController.view.frame = view.frame
        switchViewController(from: nil, to: loginViewController)
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    func login(username:String, password:String) -> [LoginResult] {
        var errors = [LoginResult]()
        if (username.isEmpty) {
            errors.append(LoginResult.MissingUsername)
        }
        if (password.isEmpty) {
            errors.append(LoginResult.MissingPassword)
        }
        if worktajmViewController?.view.superview == nil {
            if worktajmViewController == nil {
                worktajmViewController = storyboard?.instantiateViewControllerWithIdentifier("Worktajm")
                    as UITabBarController
            }
        }
        
        if (errors.count == 0) {
            switchViewController(from: loginViewController, to: worktajmViewController)
        }
        
        return errors
    }

    
    private func switchViewController(from fromVC:UIViewController?, to toVC:UIViewController?) {
        if fromVC != nil {
            fromVC!.willMoveToParentViewController(nil)
            fromVC!.view.removeFromSuperview()
            fromVC!.removeFromParentViewController()
        }
        
        if toVC != nil {
            self.addChildViewController(toVC!)
            self.view.insertSubview(toVC!.view, atIndex: 0)
            toVC!.didMoveToParentViewController(self)
        }
    }

}
