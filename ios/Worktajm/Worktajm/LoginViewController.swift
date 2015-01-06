//
//  LoginViewController.swift
//  worktajm
//
//  Created by Jim Arnell on 2015-01-05.
//  Copyright (c) 2015 Arnell Consulting AB. All rights reserved.
//

import UIKit

class LoginViewController: UIViewController {

    @IBOutlet weak var emailField: UITextField!
    @IBOutlet weak var passwordField: UITextField!
    
    override func viewDidLoad() {
        super.viewDidLoad()

        // Do any additional setup after loading the view.
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    @IBAction func onRegister(sender: AnyObject) {
        UIApplication.sharedApplication().openURL(NSURL(string: "http://worktajm.com/signup")!)
    }

    @IBAction func onLogin(sender: UIButton) {
        let parentController = self.parentViewController as RootViewController
        var loginResults:[LoginResult]
        loginResults = parentController.login(emailField.text, password: passwordField.text)
        
        if (loginResults.count > 0) {
            // Build error message
            var message = ""
            for loginResult in loginResults {
                switch (loginResult) {
                case .MissingPassword:
                    message += "Missing password"
                case .MissingUsername:
                    message += "Missing user name"
                case .InvalidCredentials:
                    message += "Invalid credentials"
                }
            }
            
            // Show alert
            let alert = UIAlertController(
                title: "Login failed",
                message: message,
                preferredStyle: .Alert)
            let cancelAction = UIAlertAction(title: "OK", style: .Default, handler: nil)
            alert.addAction(cancelAction)
            self.presentViewController(alert, animated: true, completion: nil)

        }
    }
    private func login(username:String, password:String) {
    }
    
    @IBAction func backgroundTap(sender: UIControl) {
        emailField.resignFirstResponder()
        passwordField.resignFirstResponder()
    }
    
}
